using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LogMyWork.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;

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
            //IQueryable<ProjectRole> projects = this.db.ProjectRoles.Include(r => r.Project).Where(r => r.UserID == userID).ToList();
            return View(this.db.ProjectRoles.Include(r => r.Project).Where(r => r.UserID == userID).ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            project.Roles = db.ProjectRoles.Include(r => r.User).Where(r => r.ProjectID == project.ProjectID).ToList();
            project.Roles.Insert(0, new ProjectRole { ProjectID = project.ProjectID });
            project.Tasks = db.ProjectTasks.Where(t => t.ParentProjectID == project.ProjectID).ToList();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
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
                db.ProjectRoles.Add(new ProjectRole { ProjectID = project.ProjectID, UserID = User.Identity.GetUserId()});
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
            ViewBag.Rates = new SelectList(
                this.db.Rates.Where(r => r.UserID == userID),
                "RateID",
                "RateValue",
                this.db.Entry(project).Collection(p => p.Rates).Query().Where(e => e.UserID == userID).FirstOrDefault().RateID
                );
            if (project == null)
            {
                return HttpNotFound();
            }
            ProjectEdit editProject = new ProjectEdit() { ProjectID = project.ProjectID, Name = project.Name };
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
                project.Name = form.Name;
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
