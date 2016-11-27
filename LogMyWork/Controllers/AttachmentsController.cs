using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace LogMyWork.Controllers
{
    public class AttachmentsController : Controller
    {

        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: Attachments
        public FileResult Download(int attachmentID)
        {
            string userID = User.Identity.GetUserId();
            Attachment attachment = this.db.Attachments
                .Where(a => a.AttachmentID == attachmentID)
                .Include(a => a.Task.Users)
                .Include(a => a.Task.ParentProject.Roles)
                .Where(x => x.Task.OwnerID == userID || x.Task.Users.Any(u => u.Id == userID) || x.Task.ParentProject.Roles.Any(r => r.UserID == userID)).FirstOrDefault();
            if (attachment != null)
            {

                return File(AppDomain.CurrentDomain.BaseDirectory + @"/Attachments/" + attachment.TaskID + '/' + attachment.FileName, attachment.Type);
            }
            else
            {
                return null;
            }
        }

    }
}
