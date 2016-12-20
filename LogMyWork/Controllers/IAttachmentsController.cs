using System.Web.Mvc;

namespace LogMyWork.Controllers
{
    public interface IAttachmentsController
    {
        ActionResult DeleteAttachment(int attachmentID);
        FileResult Download(int attachmentID);
    }
}