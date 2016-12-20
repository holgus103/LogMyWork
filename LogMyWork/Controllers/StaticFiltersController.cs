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
using LogMyWork.DTO.Filters;
using Commons.Time;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class StaticFiltersController : Controller, IStaticFiltersController
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        private StaticFilterCreate getCreateViewModel()
        {
            string userID = User.Identity.GetUserId();
            StaticFilterCreate viewModel = new StaticFilterCreate();

            // fill list for dropdown with projects
            var tmp = this.db.GetProjectsForUser(userID).ToList()
                .Select(p => new KeyValuePair<object, string>(p.ProjectID, p.Name)).ToList();
            tmp.Insert(0, new KeyValuePair<object, string>(null, null));
            viewModel.SelectableProjects = tmp;
            // fill list for dropdown with users
            tmp = this.db.GetRelatedUsers(userID).ToList()
                .Select(u => new KeyValuePair<object, string>(u.Id, u.Email)).ToList();
            tmp.Insert(0, new KeyValuePair<object, string>(null, null));
            viewModel.SelectableUsers = tmp;
            // fill list for dropdown with tasks
            tmp = this.db.GetAllTasksForUser(userID).ToList()
                .Select(t => new KeyValuePair<object, string>(t.TaskID, t.Name)).ToList();
            tmp.Insert(0, new KeyValuePair<object, string>(null, null));
            viewModel.SelectableTasks = tmp;
            return viewModel;
        }
        // GET: Filters
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            return View(this.db.StaticFilters
                .Where(f => f.OwnerID == userID)
                .Include(f => f.Project)
                .Include(f => f.Task)
                .Include(f => f.User)
                .ToList()
                );
        }

        // GET: Filters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PredefinedStaticFilter filter = db.StaticFilters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            if (filter.OwnerID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(filter);
        }

        // GET: Filters/Create
        public ActionResult Create()
        {
            return View(this.getCreateViewModel());
        }

        // POST: Filters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaticFilterCreateDTO filter)
        {
            if (ModelState.IsValid)
            {
                PredefinedStaticFilter newFilter = null;
                if (filter.FilterID != 0)
                {
                    newFilter = this.db.StaticFilters.Find(filter.FilterID);
                    if (newFilter.OwnerID != User.Identity.GetUserId())
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    newFilter = new PredefinedStaticFilter();
                }

                newFilter.From = UnixTime.ParseUnixTimestamp(filter.From);
                newFilter.To = UnixTime.ParseUnixTimestamp(filter.To);
                newFilter.OwnerID = User.Identity.GetUserId();
                newFilter.ProjectID = filter.ProjectID;
                newFilter.TaskID = filter.TaskID;
                newFilter.UserID = filter.UserID;
                newFilter.Name = filter.Name;
                db.StaticFilters.Add(newFilter);
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
            StaticFilterCreate viewModel = this.getCreateViewModel();
            PredefinedStaticFilter filter = db.StaticFilters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            if (filter.OwnerID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            viewModel.ProjectID = filter.ProjectID;
            viewModel.TaskID = filter.TaskID;
            viewModel.To = filter.To?.ToUnixTimestamp() ?? 0;
            viewModel.From = filter.From?.ToUnixTimestamp() ?? 0;
            viewModel.UserID = filter.UserID;
            viewModel.FilterID = filter.FilterID;
            viewModel.Name = filter.Name;
            return View("Create", viewModel);
        }


        // GET: Filters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PredefinedStaticFilter filter = db.StaticFilters
                .Where(f => f.FilterID == id.Value)
                .Include(f => f.Project)
                .Include(f => f.Task)
                .Include(f => f.User)
                .FirstOrDefault();
            if (filter == null)
            {
                return HttpNotFound();
            }
            if (filter.OwnerID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(filter);
        }

        // POST: Filters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PredefinedStaticFilter filter = db.StaticFilters.Find(id);
            if (filter.OwnerID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.StaticFilters.Remove(filter);
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
