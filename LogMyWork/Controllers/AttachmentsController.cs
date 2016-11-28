using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class AttachmentsController : AjaxController
    {

        private LogMyWorkContext db = new LogMyWorkContext();

        private Attachment loadAttachmentWithPermissionCheck(int attachmentID)
        {
            string userID = User.Identity.GetUserId();
            return this.db.Attachments
                .Where(a => a.AttachmentID == attachmentID)
                .Include(a => a.Task.Users)
                .Include(a => a.Task.ParentProject.Roles)
                .Where(x => x.Task.OwnerID == userID || x.Task.Users.Any(u => u.Id == userID) || x.Task.ParentProject.Roles.Any(r => r.UserID == userID))
                .FirstOrDefault();
        }
        // GET: Attachments
        public FileResult Download(int attachmentID)
        {

            Attachment attachment = this.loadAttachmentWithPermissionCheck(attachmentID);
            if (attachment != null)
            {

                return File(AppDomain.CurrentDomain.BaseDirectory + @"/Attachments/" + attachment.TaskID  + attachment.FileName, attachment.Type);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult DeleteAttachment(int attachmentID)
        {
            Attachment attachment = this.loadAttachmentWithPermissionCheck(attachmentID);
            if (attachment != null)
            {
                //this.db.Attachments.Attach(attachment);
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"Attachments/" + attachment.TaskID + attachment.FileName);
                this.db.Attachments.Remove(attachment);
                this.db.SaveChanges();
                return this.ajaxSuccess();
            }
            else
            {
                return this.ajaxFailure();
            }
        }

    }
}
