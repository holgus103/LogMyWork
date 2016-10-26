using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogMyWork.Models;

namespace LogMyWork.Controllers
{
    public class ProjectRolesController : Controller
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: ProjectRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(db.ProjectRoles.Include( p => p.User).Where(t => t.ProjectID == id));
        }

        // GET: ProjectRoles/Create
        public ActionResult Create(int? id)
        {
            ViewBag.ProjectID = id;
            ViewBag.UserID = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: ProjectRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,UserID,Role")] ProjectRole projectRole)
        {
            if (ModelState.IsValid)
            {
                db.ProjectRoles.Add(projectRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", projectRole.ProjectID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Email", projectRole.UserID);
            return View(projectRole);
        }


        // GET: ProjectRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRole projectRole = db.ProjectRoles.Find(id);
            if (projectRole == null)
            {
                return HttpNotFound();
            }
            return View(projectRole);
        }

        // POST: ProjectRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectRole projectRole = db.ProjectRoles.Find(id);
            db.ProjectRoles.Remove(projectRole);
            db.SaveChanges();
            return RedirectToAction("Index");
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
