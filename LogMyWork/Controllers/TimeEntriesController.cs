﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogMyWork.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using LogMyWork.Consts;
using System.Web.Script.Serialization;
//using System.Threading.Tasks;
using Commons;
using LogMyWork.ViewModels.TimeEntries;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class TimeEntriesController : AjaxController
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: TimeEntries
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            TimeEntryIndex viewModel = new TimeEntryIndex();
            viewModel.TimeEntries = this.db.TimeEntries.Include(t => t.ParentTask.ParentProject).Where(e => e.UserID == userId).ToList();
            viewModel.Roles = this.db.Users.Include(u => u.ProjectRoles).Where(u => u.Id == userId).SelectMany(u => u.ProjectRoles).ToList();
            viewModel.Roles.Insert(0, new ProjectRole());

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult GetFilteredValues(long? from, long? to, int? projectID)
        {
            DateTime dateFrom;
            if (from != null)
            {
                dateFrom = UnixTime.ParseUnitTimestamp((ulong)from);
            }
            else
            {
                dateFrom = UnixTime.ParseUnitTimestamp(0);
            }

            string userId = User.Identity.GetUserId();
            var entries = this.db.TimeEntries.Include(t => t.ParentTask.ParentProject).Where(t => t.UserID == userId && t.Start > dateFrom);
            if (to != null)
            {
                DateTime dateTo = UnixTime.ParseUnitTimestamp((ulong)to);
                entries = entries.Where(t => t.End < dateTo);
            }
            if (projectID != null)
            {
                entries = entries.Where(e => e.ParentTask.ParentProjectID == projectID);
            }
            return PartialView("~/Views/Partials/TimeEntriesResultsTable.cshtml", entries.ToList());
        }
        // GET: TimeEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            if (timeEntry == null)
            {
                return HttpNotFound();
            }
            return View(timeEntry);
        }

        // GET: TimeEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimeEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParentTaskId")] TimeEntry timeEntry)
        {
            ProjectTask task;
            var entries = db.TimeEntries.Where(t => t.Active == true && t.ParentTaskID == timeEntry.ParentTaskID);
            // if exits a currently active entry for this task, end it
            if (entries.Count() > 0)
            {
                foreach (var val in entries)
                {
                    val.Active = false;
                    val.End = DateTime.UtcNow;
                }
                task = this.db.ProjectTasks.Find(timeEntry.ParentTaskID);
                task.Status = TaskStatus.InProgress;
                Session[SessionKeys.CurrentTimeEntry] = null;
                db.SaveChanges();
                return this.ajaxSuccess();
            }
            //else create new record and save it

            Session[SessionKeys.CurrentTimeEntry] = timeEntry;
            timeEntry.Active = true;
            timeEntry.Start = DateTime.UtcNow;
            timeEntry.UserID = User.Identity.GetUserId();
            task = this.db.ProjectTasks.Find(timeEntry.ParentTaskID);
            task.Status = TaskStatus.CurrentlyInProgress;
            if (ModelState.IsValid)
            {
                db.TimeEntries.Add(timeEntry);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return this.ajaxFailure();
                }
                return this.ajaxSuccess();
            }

            return this.ajaxFailure();
        }

        // GET: TimeEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            if (timeEntry == null)
            {
                return HttpNotFound();
            }
            return View(timeEntry);
        }

        // POST: TimeEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntryID,Start,End,Active")] TimeEntry timeEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeEntry);
        }

        // GET: TimeEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            if (timeEntry == null)
            {
                return HttpNotFound();
            }
            return View(timeEntry);
        }

        // POST: TimeEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            db.TimeEntries.Remove(timeEntry);
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
