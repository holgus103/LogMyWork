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
using System.Threading.Tasks;

namespace LogMyWork.Controllers
{
    public class TimeEntriesController : AjaxController
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: TimeEntries
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            foreach (var t in this.db.TimeEntries.Where(val => val.UserID == userId))
            {
                if (t.ParentTask == null)
                {
                    t.ParentTask = this.db.ProjectTasks.Find(t.ParentTaskID);
                }
            }

            return View(db.TimeEntries.ToList());
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
            Debug.WriteLine("REQUEST");
            foreach (var val in db.TimeEntries.Where(t => t.Active == true))
            {
                val.Active = false;
                val.End = DateTime.UtcNow;
            }

            // if exits a currently active entry for this task, end it
            if (db.TimeEntries
                .Where(t => t.Active == true && t.ParentTaskID == timeEntry.ParentTaskID)
                .Count() > 0)
            {
                Session[SessionKeys.CurrentTimeEntry] = null;
                db.SaveChanges();
                return this.ajaxSuccess();
            }
            //else create new record and save it

            Session[SessionKeys.CurrentTimeEntry] = timeEntry;
            timeEntry.Active = true;
            timeEntry.Start = DateTime.UtcNow;
            timeEntry.UserID = User.Identity.GetUserId();

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
