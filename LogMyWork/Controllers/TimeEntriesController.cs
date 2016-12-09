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
using System.Diagnostics;
using LogMyWork.Consts;
using System.Web.Script.Serialization;
//using System.Threading.Tasks;
using Commons.Time;
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
            string userID = User.Identity.GetUserId();
            TimeEntryIndex viewModel = new TimeEntryIndex();
            viewModel.StaticFilters = this.db.StaticFilters
                .Where(f => f.OwnerID == userID)
                .ToList();

            viewModel.RelativeFilters = this.db.RelativeFilters
                    .Where(f => f.OwnerID == userID)
                .ToList();

            viewModel.TimeEntries.TimeEntries = this.db.TimeEntries
                .Include(t => t.ParentTask.ParentProject.Rates)
                .Include(e => e.User)
                .Where(e => e.UserID == userID).ToList();

            viewModel.TimeEntries.Sum = TimeSpan.FromSeconds(viewModel.TimeEntries.TimeEntries.Sum(e => e.Duration.Value.TotalSeconds));

            viewModel.TimeEntries.TotalEarned = viewModel.TimeEntries.TimeEntries.Sum(e => e.Charge);

            viewModel.Projects = viewModel.TimeEntries.TimeEntries.Select(e => e.ParentTask.ParentProject).Distinct().ToList();
            // insert empty Project for DropDown
            viewModel.Projects.Insert(0, null);
            viewModel.Tasks = this.db.ProjectTasks
                .Include(t => t.Users)
                .Where(t => t.OwnerID == userID || t.Users.Any(u => u.Id == userID))
                .ToList();
            // insert empty Task for DropDown
            viewModel.Tasks.Insert(0, null);
            viewModel.Users = this.db.Projects
                        .Include(p => p.Roles.Select(r => r.User))
                        .Where(p => p.Roles.Any(r => r.Role == Role.Owner && r.UserID == userID))
                        .SelectMany(p => p.Roles.Select(r => r.User))
                        .Union(this.db.ProjectTasks
                                .Include(t => t.Users)
                                .Where(t => t.OwnerID == userID)
                                .SelectMany(t => t.Users)
                                )
                        .Union(this.db.Users.Where(u => u.Id == userID))
                        .ToList();
            // insert empty User for DropDown
            viewModel.Users.Insert(0, null);


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult GetFilteredValues(long? from, long? to, int? projectID, int? taskID, string user)
        {
            DateTime dateFrom;
            if (from != null)
            {
                dateFrom = UnixTime.ParseUnixTimestamp((ulong)from);
            }
            else
            {
                dateFrom = UnixTime.ParseUnixTimestamp(0);
            }

            string userId = User.Identity.GetUserId();
            TimeEntriesTable viewModel = new TimeEntriesTable();
            var entries = this.db.TimeEntries
                .Include(t => t.ParentTask.ParentProject.Roles)
                .Include(t => t.ParentTask.ParentProject.Rates)
                .Include(e => e.User)
                .Where(e => e.ParentTask.OwnerID == userId || e.UserID == userId || e.ParentTask.ParentProject.Roles.Any(r => r.Role == Role.Owner && r.UserID == userId))
                .Where(t => t.Start > dateFrom);
            if (to != null)
            {
                DateTime dateTo = UnixTime.ParseUnixTimestamp((ulong)to);
                entries = entries.Where(t => t.End < dateTo);
            }
            if (projectID != null)
            {
                entries = entries.Where(e => e.ParentTask.ParentProjectID == projectID);
            }
            if (taskID != null)
            {
                entries = entries.Where(e => e.ParentTaskID == taskID);
            }
            if (!String.IsNullOrWhiteSpace(user))
            {
                entries = entries.Where(e => e.UserID == user);
            }
            viewModel.TimeEntries = entries;
            viewModel.Sum = TimeSpan.FromSeconds(viewModel.TimeEntries.Sum(e => e.Duration.Value.TotalSeconds));
            viewModel.TotalEarned = viewModel.TimeEntries.Sum(e => e.Charge);
            return PartialView("~/Views/Partials/TimeEntriesResultsTable.cshtml", viewModel);
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
