using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogMyWork.Controllers
{
    public abstract class AjaxController : Controller
    {
        protected ContentResult ajaxSuccess()
        {
            return this.Content("1");
        }

        protected ContentResult ajaxFailure()
        {
            return this.Content("0");
        }

        protected ContentResult sendID(int id)
        {
            return this.Content(id.ToString());
        }
    }
}