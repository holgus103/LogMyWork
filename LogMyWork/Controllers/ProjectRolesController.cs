using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogMyWork.Models;
using LogMyWork.ViewModels.ProjectRoles;
using LogMyWork.ContextExtensions;
using Microsoft.AspNet.Identity;
using LogMyWork.Consts;
using LogMyWork.DTO.ProjectRoles;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class ProjectRolesController : Controller
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: ProjectRoles/Create
        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            string userID = User.Identity.GetUserId();
            // if user is neither an admin nor a manager => refuse access
            if(!this.db.HasProjectRole(id.Value, userID, Role.Manager))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ProjectRoleCreate viewModel = new ProjectRoleCreate();
            viewModel.SelectableUsers = this.db.Users
                .Include(u => u.ProjectRoles)
                .Where(u => !u.ProjectRoles.Any(r => r.ProjectID == id.Value))
                .ToList()
                .Select(u => new KeyValuePair<object, string>(u.Id, u.Email));

            viewModel.SelectableRoles = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .Where(r => r != Role.Owner);
            viewModel.ProjectID = id.Value;

               
            return View(viewModel);
        }

        // POST: ProjectRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectRoleCreateDTO dto)
        {
            string userID = User.Identity.GetUserId();
            if (!this.db.HasProjectRole(dto.ProjectID, userID, Role.Manager))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (ModelState.IsValid)
            {
                ProjectRole role = new ProjectRole()
                {
                    ProjectID = dto.ProjectID,
                    Role = dto.Role,
                    UserID = dto.UserID
                };
                db.ProjectRoles.Add(role);
                db.SaveChanges();
                return this.RedirectToAction("Index", "Projects");
            }
            return View();
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
            if (!this.db.HasProjectRole(projectRole.ProjectID, User.Identity.GetUserId(), Role.Owner))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else
                return View(projectRole);
        }

        // POST: ProjectRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectRole projectRole = db.ProjectRoles.Find(id);
            if (!this.db.HasProjectRole(projectRole.ProjectID, User.Identity.GetUserId(), Role.Owner))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.ProjectRoles.Remove(projectRole);
            db.SaveChanges();
            return this.RedirectToAction("Index", "Projects"); 
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
