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
using LogMyWork.DTO.Projects;
using LogMyWork.ContextExtensions;
using LogMyWork.ViewModels.Tasks;
using System;
using LogMyWork.Tools;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class ProjectsController : AjaxController
    {
        private LogMyWorkContext db = new LogMyWorkContext();


        // GET: Projects
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            List<ProjectRole> projects = this.db.ProjectRoles.Include(r => r.Project).Where(r => r.UserID == userID && r.Project.Status != ProjectStatus.Completed).ToList();
            return View(projects);
        }

        public ActionResult Archive()
        {
            string userID = User.Identity.GetUserId();
            List<ProjectRole> projects = this.db.ProjectRoles.Include(r => r.Project).Where(r => r.UserID == userID && r.Project.Status == ProjectStatus.Completed).ToList();
            return View("Index", projects);
        }

        public ActionResult GetUsersForProject(int projectID)
        {   // TODO
            // check what happens if project does not exist
            var data = this.db.GetUsersForProjectAsKeyValuePair(projectID).ToList();
            data.Insert(0, new KeyValuePair<object, string>(0, null));
            return PartialView("~/Views/Partials/SelectOptionsTemplate.cshtml", data);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetProjects(int taskID, string userID)
        {
            IQueryable<Project> res = this.db.GetProjectsForUser(userID);

            //if(projectID > 0)
            //{
            //    res = res.Where(p => p.ProjectID == projectID);
            //}

            if (!String.IsNullOrWhiteSpace(userID))
            {
                res = res.Include(p => p.Roles)
                    .Where(p => p.Roles.Any(r => r.UserID == userID));
            }

            if(taskID > 0)
            {
                res = res.Include(p => p.Tasks)
                    .Where(p => p.Tasks.Any(t => t.TaskID == taskID));
            }
            var data = res.ToList().Select(p => new KeyValuePair<object, string>(p.ProjectID, p.Name)).ToList();
            data.Insert(0, new KeyValuePair<object, string>(0, null));
            return PartialView("~/Views/Partials/SelectOptionsTemplate.cshtml", data);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProjectDetails projectDetails = new ProjectDetails();
            string userID = User.Identity.GetUserId();
            projectDetails.CurrentProjectRole = this.db.GetUserRoleForProject(id.Value, userID);
            if (projectDetails.CurrentProjectRole == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            projectDetails.Project = db.Projects
                .Where(p => p.ProjectID == id)
                .Include(p => p.Tasks)
                .Include(p => p.Roles.Select(r => r.User))
                .FirstOrDefault();
            projectDetails.Tasks = new TaskIndex();
            // load all assgned tasks in this project
            projectDetails.Tasks.AssignedTasks = this.db.Users.Where(u => u.Id == userID)
                .Include(u => u.Tasks)
                .SelectMany(u => u.Tasks.Where(t => t.ParentProjectID == id.Value))
                .ToList();

            // insert empty user to roles for dropdowns 
            //projectDetails.Project.Roles.Insert(0, new ProjectRole { ProjectID = projectDetails.Project.ProjectID });

            return View(projectDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int? id, ProjectStatus status)
        {
            if(id == null)
            {
                return this.ajaxFailure();
            }

            if (!this.db.HasProjectRole(id.Value, User.Identity.GetUserId(), Role.Owner))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var project = this.db.Projects.Find(id);
            project.Status = status;
            this.db.SaveChanges();
            return this.ajaxSuccess();

        }
        // GET: Projects/Create
        public ActionResult Create()
        {
            string userID = this.User.Identity.GetUserId();
            ProjectCreate viewModel = new ProjectCreate();
            viewModel.UserRates = this.db.GetUserRatesAsKeyValuePair(userID);
            viewModel.Users = this.db.GetUsersAsKeyValuePair();
            return View(viewModel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectCreateDTO form)
        {
            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                Project project;
                // existing project
                if (form.ProjectID > 0)
                {
                    // refuse edit if user is not the owner
                    if (!this.db.HasProjectRole(form.ProjectID, userID, Role.Owner))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    project = this.db.Projects
                        .Where(p => p.ProjectID == form.ProjectID)
                        .Include(p => p.Rates).First();
                    
                }
                // new project
                else {
                    project = new Project();
                }
                project.Name = form.Name;
                if(project.Rates == null)
                {
                    project.Rates = new List<Rate>();
                }
                // if the rate is not already contained, add it and remove the old one for the current user
                if (!project.Rates.Contains(form.Rate, new PropertyComparer<Rate, int>(r => r.RateID)))
                {
                    this.db.Rates.Attach(form.Rate);
                    project.Rates.Add(form.Rate);
                    project.Rates.RemoveAll(r => r.UserID == userID);
                }
                // add a owner entry if the project is new
                if(form.ProjectID == 0)
                {
                    db.Projects.Add(project);
                    // save to get id
                    db.SaveChanges();
                    this.db.ProjectRoles.Add(new ProjectRole() { ProjectID = project.ProjectID, UserID = userID, Role = Role.Owner });
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View( form);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userID = User.Identity.GetUserId();
            // if not project user => edition is refused
            if(!this.db.HasProjectRole(id.Value, userID, Role.Owner))
            {
                return  new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ProjectCreate viewModel = new ProjectCreate();
            //viewModel.Role = this.db.ProjectRoles.Where(r => r.ProjectID == id && r.UserID == userID).FirstOrDefault();
            viewModel.Name = project.Name;
            viewModel.UserRates = this.db.GetUserRatesAsKeyValuePair(userID);
            viewModel.Rate = this.db.GetRateForProjectForUser(id.Value, userID);
            viewModel.ProjectID = id.Value;

            return View("Create",viewModel);
        }

        public ActionResult Users(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userID = User.Identity.GetUserId();

            ProjectUsers viewModel = this.db.Projects
                .Include(p => p.Roles.Select(t => t.User))
                .Where(p => p.ProjectID == id)
                .ToList()
                .Select(p =>new ProjectUsers()
                {
                    ProjectName = p.Name,
                    Users = p.Roles,
                    ProjectID = p.ProjectID,
                    CurrentUserRole = p.Roles.Where(r => r.UserID == userID).FirstOrDefault()

                })
                .First();
            // if not a project member => refuse access
            if(viewModel.CurrentUserRole == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(viewModel);
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
