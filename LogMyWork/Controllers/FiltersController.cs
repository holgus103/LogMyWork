using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LogMyWork.Models;
using Microsoft.AspNet.Identity;
using LogMyWork.ViewModels.Filters;
using LogMyWork.ContextExtensions;

namespace LogMyWork.Controllers
{
    public class FiltersController : Controller
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: Filters
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            return View(this.db.Filters.Where(f => f.OwnerID == userID).ToList());
        }

        // GET: Filters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PredefinedFilter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // GET: Filters/Create
        public ActionResult Create()
        {
            string userID = User.Identity.GetUserId();
            FilterCreate viewModel = new FilterCreate();
            viewModel.SelectableProjects = this.db.GetProjectsForUser(userID).ToList()
                .Select(p => new KeyValuePair<object, string>(p.ProjectID, p.Name));
            viewModel.SelectableUsers = this.db.GetRelatedUsers(userID).ToList()
                .Select(u => new KeyValuePair<object, string>(u.Id, u.Email));
            viewModel.SelectableTasks = this.db.GetAllTasksForUser(userID).ToList()
                .Select(t => new KeyValuePair<object, string>(t.TaskID, t.Name));
            return View(viewModel);
        }

        // POST: Filters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilterID,OwnerID,TaskID,ProjectID,UserID,From,To,FilterType")] PredefinedFilter filter)
        {
            if (ModelState.IsValid)
            {
                db.Filters.Add(filter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filter);
        }

        // GET: Filters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PredefinedFilter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // POST: Filters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilterID,OwnerID,TaskID,ProjectID,UserID,From,To,FilterType")] PredefinedFilter filter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filter);
        }

        // GET: Filters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PredefinedFilter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // POST: Filters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PredefinedFilter filter = db.Filters.Find(id);
            db.Filters.Remove(filter);
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
