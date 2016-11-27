using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

using LogMyWork.Consts;
using LogMyWork.Models;
using LogMyWork.ViewModels.Projects;
using System;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        private bool isProjectOwner(int projectID, string userID)
        {
            return this.db.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID && r.Role == Role.Owner).Count() > 0;
        }

        // GET: Projects
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            List<ProjectRole> projects = this.db.ProjectRoles.Include(r => r.Project).Where(r => r.UserID == userID).ToList();
            return View(projects);
        }

        public ActionResult GetUsersForProject(int projectID)
        {
             var data = this.db.Projects
                .Where(x => x.ProjectID == projectID)
                .Include(x => x.Roles.Select(r => r.User))
                .ToList()
                .SelectMany(x => x.Roles.Select(r => new Tuple<object, string>(r.User.Id, r.User.Email)))
                .ToList();
            data.Insert(0, new Tuple<object, string>(null, null));
            return PartialView("~/Views/Partials/SelectOptionsTemplate.cshtml", data);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            ProjectDetails projectDetails = new ProjectDetails();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userId = User.Identity.GetUserId();
            projectDetails.CurrentProjectRole = this.db.ProjectRoles
                .Include(r => r.User.OwnedTasks)
                .Include(r => r.User.Tasks)
                .Where(r => r.UserID == userId && r.ProjectID == id)
                .ToList()
                .Select(r =>  new ProjectRole {
                    ProjectRoleID = r.ProjectRoleID,
                    ProjectID = r.ProjectID,
                    Role = r.Role,
                    User = new ApplicationUser {
                        OwnedTasks = r.User.OwnedTasks.Where(t => t.ParentProjectID == id).ToList(),
                        Tasks = r.User.Tasks.Where(t => t.ParentProjectID == id).ToList()
                    }
                })
                .FirstOrDefault();
            if (projectDetails.CurrentProjectRole == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            projectDetails.Project = db.Projects.Include(p => p.Tasks).Include(p => p.Roles.Select(r => r.User)).Where(p => p.ProjectID == id).FirstOrDefault();


            if (projectDetails.Project == null)
            {
                return HttpNotFound();
            }

            // insert empty user to roles for dropdowns 
            projectDetails.Project.Roles.Insert(0, new ProjectRole { ProjectID = projectDetails.Project.ProjectID });

            return View(projectDetails);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            string userID = this.User.Identity.GetUserId();
            ViewBag.Rates = new SelectList(this.db.Rates.Where(r => r.UserID == userID), "RateID", "RateValue");
            return View("Edit");
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectEdit form)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project() { Name = form.Name };
                project.Rates = new List<Rate>() { this.db.Rates.Find(form.RateID) };
                db.Projects.Add(project);
                db.ProjectRoles.Add(new ProjectRole { ProjectID = project.ProjectID, UserID = User.Identity.GetUserId() });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", form);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userID = User.Identity.GetUserId();
            Project project = db.Projects.Find(id);
            ProjectRole role = this.db.Entry(project).Collection(p => p.Roles).Query().Where(r => r.UserID == userID).FirstOrDefault();
            ViewBag.Rates = new SelectList(
                this.db.Rates.Where(r => r.UserID == userID),
                "RateID",
                "RateValue",
                this.db.Entry(project).Collection(p => p.Rates).Query().Where(e => e.UserID == userID).FirstOrDefault()?.RateID
                );
            if (project == null)
            {
                return HttpNotFound();
            }
            ProjectEdit editProject = new ProjectEdit() { ProjectID = project.ProjectID, Name = project.Name, Role = role };
            return View(editProject);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "ProjectID,Name,Rates[0].RateID")] Project project
        public ActionResult Edit(ProjectEdit form)
        {
            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                Project project = this.db.Projects.Find(form.ProjectID);
                // get rate for this project for this user
                Rate rate = this.db.Rates.Include(r => r.Projects).Where(r => r.UserID == userID && r.Projects.Any(p => p.ProjectID == project.ProjectID)).FirstOrDefault();
                // update project fields
                if (this.isProjectOwner(form.ProjectID.Value, userID))
                {
                    project.Name = form.Name;
                }
                // load project rates
                this.db.Entry(project).Collection(p => p.Rates).Load();
                // remove previously selected rate
                project.Rates.Remove(rate);
                // add new relation for RateProject
                project.Rates.Add(this.db.Rates.Find(form.RateID));
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!this.isProjectOwner(id.Value, User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!this.isProjectOwner(id, User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Users(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(this.db.Projects.Include(p => p.Roles.Select(t => t.User)).Where(p => p.ProjectID == id).FirstOrDefault());
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
