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

namespace LogMyWork.Controllers
{
    [Authorize]
    public class TasksController : AjaxController
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: Tasks
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            ProjectRole role = new ProjectRole();
            role.Role = Role.Worker;
            role.User = db.Users
                .Where(u => u.Id == userID)
                .Include(u => u.Tasks)
                .Include(u => u.OwnedTasks)
                .SingleOrDefault();
            return View(role);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Include(t => t.ParentProject).Include(t => t.Users).Include( t => t.Owner).Where(t => t.TaskID == id).FirstOrDefault();
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            //projectTask.ParentProject = db.Projects.Find(projectTask.ParentProjectID);
            return View(projectTask);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.ParentProjectID = new SelectList(db.Projects, "ProjectID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, TaskStatus status)
        {
            var task = this.db.ProjectTasks.Find(id);
            task.Status = status;
            db.SaveChanges();
            return this.ajaxSuccess();
        } 
        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskID,Name,ParentProjectID,Users")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                task.Users.RemoveAt(task.Users.Count - 1);
                task.Users.ForEach(u => this.db.Users.Attach(u));
                //ProjectTask task = new ProjectTask { Name = projectTaskEdit.Name, ParentProjectID = projectTaskEdit.ParentProjectID, Users = new List<ApplicationUser>()};
                //projectTaskEdit.Users.ForEach(u => task.Users.Add(this.db.Users.Find(u)));
                task.Status = task.Users.Count() > 0 ? TaskStatus.Assigned : TaskStatus.Created;
                task.OwnerID = User.Identity.GetUserId();
                db.ProjectTasks.Add(task);
                db.SaveChanges();
                return this.sendID(task.TaskID);
            }

            return this.ajaxFailure();
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ParentProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectTask.ParentProjectID);
            return View(projectTask);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskID,Name,ParentProjectID")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectTask.ParentProjectID);
            return View(projectTask);
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
