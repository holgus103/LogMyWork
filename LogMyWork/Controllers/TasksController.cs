using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogMyWork.Models;
using Microsoft.AspNet.Identity;
using LogMyWork.Consts;
using LogMyWork.ViewModels.Tasks;
using System.IO;
using System.Globalization;
using LogMyWork.ContextExtensions;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class TasksController : AjaxController
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        private void prepareCreateViewModel(TaskCreate viewModel, string userID = null)
        {
            if (userID == null)
                userID = User.Identity.GetUserId();
            viewModel.SelectableProjects = db.ProjectRoles
                .Where(r => r.UserID == userID && r.Role < Role.Worker)
                .Include(r => r.Project)
                .ToList()
                .Select(r => new KeyValuePair<object, string>(r.ProjectID, r.Project.Name))
                .ToList();
            int projectID = viewModel.ParentProjectID == 0? (int)viewModel.SelectableProjects.First().Key : viewModel.ParentProjectID;

            viewModel.SelectableUsers = this.db.Projects
                .Where(x => x.ProjectID == projectID)
                .Include(x => x.Roles.Select(r => r.User))
                .First()
                .Roles
                .Select(r => new KeyValuePair<object, string>(r.User.Id, r.User.Email))
                .ToList();
            viewModel.SelectableUsers.Insert(0, new KeyValuePair<object, string>(null, null));
        }
        // GET: Tasks
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            TaskIndex viewModel = this.db.Users
                .Where(u => u.Id == userID)
                .Include(u => u.Tasks)
                .Include(u => u.OwnedTasks)
                .ToList()
                .Select(u => new TaskIndex() { OwnedTasks = u.OwnedTasks, AssignedTasks = u.Tasks })
                .FirstOrDefault();
            return View(viewModel);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks
                .Where(t => t.TaskID == id)
                .Include(t => t.ParentProject)
                .Include(t => t.Users)
                .Include(t => t.Owner)
                .Include(t => t.Attachments)
                .FirstOrDefault();
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            //projectTask.ParentProject = db.Projects.Find(projectTask.ParentProjectID);
            return View(projectTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, TaskStatus status)
        {
            var task = this.db.ProjectTasks.Find(id);
            task.Status = status;
            db.SaveChanges();
            return this.ajaxSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTasksForProject(int projectID)
        {
            return View("~/Partials/SelectOptionsTemplate.cshtml", this.db.ProjectTasks
                .Where(t => t.ParentProjectID == projectID)
                .ToList()
                .Select(t => new KeyValuePair<int, string>(t.TaskID, t.Name))
                );
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetTasks(int projectID, string userID)
        {
            IQueryable<ProjectTask> res = this.db.GetAllTasksForUser(User.Identity.GetUserId());
            if(projectID > 0)
            {
                res = res.Where(t => t.ParentProjectID == projectID);
            }

            //if(taskID > 0)
            //{
            //    res = res.Where(t => t.TaskID == taskID);
            //}

            if(!String.IsNullOrWhiteSpace(userID))
            {
                res = res.Include(t => t.Users)
                    .Where(t => t.Users.Any(u => u.Id == userID));
            }
            var data = res.ToList().Select(t => new KeyValuePair<object, string>(t.TaskID, t.Name)).ToList();
            data.Insert(0, new KeyValuePair<object, string>(0, null));
            return PartialView("~/Views/Partials/SelectOptionsTemplate.cshtml", data);
        }
        // GET: Tasks/Create
        public ActionResult Create()
        {

            TaskCreate viewModel = new TaskCreate();
            // Load projects where the user has management rights
            this.prepareCreateViewModel(viewModel);
            return View(viewModel);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskCreateDTO form)
        {
            if (ModelState.IsValid)
            {
                if (form?.Users.Count > 0)
                    form.Users.RemoveAt(form.Users.Count - 1);
                ProjectTask task;
                if (form.TaskID != 0)
                {
                    task = this.db.ProjectTasks
                        .Where(x => x.TaskID == form.TaskID)
                        .Include(x => x.Users)
                        .FirstOrDefault();

                    // remove users that are no longer assigned to the task
                    task.Users.RemoveAll(x => !form.Users.Contains(x, new ApplicationUser.IdComparer()));
                    // remove duplicates
                    form.Users.RemoveAll(x => task.Users.Contains(x, new ApplicationUser.IdComparer()));
                    
                }
                else {
                    task = new ProjectTask();
                    task.Created = DateTime.UtcNow;
                }
                // task not found
                if(task == null)
                {
                    return HttpNotFound();
                }
                if(task.Users == null)
                {
                    task.Users = new List<ApplicationUser>();
                }
                task.LastModified = DateTime.UtcNow;
                task.Deadline = DateTime.ParseExact(form.Deadline, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture).ToUniversalTime();
                task.Description = form.Description;
                task.Name = form.Name;
                task.OwnerID = User.Identity.GetUserId();
                task.ParentProjectID = form.ParentProjectID;
                task.Status = form.Users.Count() > 0 ? TaskStatus.Assigned : TaskStatus.Created;
                form.Users.ForEach(u => this.db.Users.Attach(u));
                form.Users.ForEach(x => task.Users.Add(x));
                List<Attachment> attachments = new List<Attachment>();
                if(task.TaskID == 0)
                {
                    db.ProjectTasks.Add(task);
                }
                db.SaveChanges();
                
                // save all attachments
                foreach (var val in form.Files)
                {
                    if (val != null)
                    {
                        db.Attachments.Add(new Attachment() { FileName = val.FileName, Size = val.ContentLength, Type = val.ContentType, TaskID = task.TaskID });
                        DirectoryInfo dir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Attachments/" + task.TaskID);
                        val.SaveAs(dir.FullName + @"/" + val.FileName);
                    }
                }

                db.SaveChanges();
                return this.Redirect("/Projects/Details/" + form.ParentProjectID);
            }

            return View();
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks
                .Where(t => t.TaskID == id)
                .Include(t => t.Users)
                .Include(t => t.Attachments)
                .FirstOrDefault();
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            TaskCreate viewModel = new TaskCreate()
            {
                Deadline = projectTask.Deadline.ToString("MM/dd/yyyy h:mm tt"),
                Users = projectTask.Users,
                TaskID = projectTask.TaskID,
                Description = projectTask.Description,
                Name = projectTask.Name,
                ParentProjectID = projectTask.ParentProjectID

            };
            this.prepareCreateViewModel(viewModel);
            return View("Create", viewModel);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            db.ProjectTasks.Remove(projectTask);
            db.SaveChanges();
            return RedirectToAction("Details", "Projects", new { id = projectTask.ParentProjectID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
