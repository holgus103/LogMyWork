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
using LogMyWork.DTO.Projects;
using LogMyWork.ContextExtensions;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private LogMyWorkContext db = new LogMyWorkContext();


        // GET: Projects
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            List<ProjectRole> projects = this.db.ProjectRoles.Include(r => r.Project).Where(r => r.UserID == userID).ToList();
            return View(projects);
        }

        public ActionResult GetUsersForProject(int projectID)
        {
            var data = this.db.GetUsersForProjectAsKeyValuePair(projectID).ToList();
            data.Insert(0, new KeyValuePair<object, string>(null, null));
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
                if (!project.Rates.Contains(form.Rate, new Rate.IdComparer()))
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
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ProjectCreate viewModel = new ProjectCreate();
            viewModel.Role = this.db.ProjectRoles.Where(r => r.ProjectID == id && r.UserID == userID).FirstOrDefault();
            viewModel.Name = project.Name;
            viewModel.UserRates = this.db.GetUserRatesAsKeyValuePair(userID);
            viewModel.Rate = this.db.GetRateForProjectForUser(id.Value, userID);
            viewModel.ProjectID = id.Value;

            return View("Create",viewModel);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!this.db.isProjectOwner(id.Value, User.Identity.GetUserId()))
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
            if (!this.db.isProjectOwner(id, User.Identity.GetUserId()))
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
