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
using LogMyWork.ContextExtensions;
using LogMyWork.ViewModels.Issues;
using LogMyWork.DTO.Issues;
using LogMyWork.Consts;

namespace LogMyWork.Controllers
{
    public class IssuesController : Controller
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        private IssueCreate getCreateModel()
        {
            IssueCreate viewModel = new IssueCreate();
            viewModel.SelectableProjects = this.db.GetProjectsForUser(User.Identity.GetUserId())
                .ToList()
                .Select(p => new KeyValuePair<object, string>(p.ProjectID, p.Name));
            return viewModel;
        }
        // GET: Issues
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var issues = this.db.ProjectRoles.Where(r => r.UserID == userID)
                .Include(r => r.Project.Issues)
                .SelectMany(r => r.Project.Issues);
            return View(issues.ToList());
        }

        // GET: Issues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            if (!this.db.HasProjectAccess(issue.ProjectID, User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(issue);
        }

        // GET: Issues/Create
        public ActionResult Create()
        {
            return View(this.getCreateModel());
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssueCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                Issue issue = null;
                if (!this.db.HasProjectAccess(dto.ProjectID, userID))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (dto.IssueID > 0)
                {
                    issue = this.db.Issues.Find(dto.IssueID);
                    if (issue == null ||
                        !this.db.HasProjectAccess(issue.ProjectID, userID) ||
                        (issue.ReporterID != userID && !this.db.HasProjectRole(issue.ProjectID, userID, Role.Manager))
                        )
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    issue = new Issue();
                    issue.RaportDate = DateTime.UtcNow;
                    issue.ReporterID = userID;
                    issue.Status = IssueStatus.Reported;
                    issue.IssueNumber = this.db.Issues.Where(i => i.ProjectID == dto.ProjectID).Count() + 1;
                }

                issue.ProjectID = dto.ProjectID;
                issue.Description = dto.Description;
                issue.LastModified = DateTime.UtcNow;
                issue.Title = dto.Title;

                if(dto.IssueID == 0)
                db.Issues.Add(issue);
                db.SaveChanges();
                foreach ()
                    return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", issue.ProjectID);
            return View(issue);
        }

        // GET: Issues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", issue.ProjectID);
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IssueID,IssueNumber,ProjectID,Description,ReporterID,RaportDate,LastModified,Status")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Name", issue.ProjectID);
            return View(issue);
        }

        // GET: Issues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Issue issue = db.Issues.Find(id);
            db.Issues.Remove(issue);
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
