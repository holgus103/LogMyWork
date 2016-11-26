using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Tasks
{
    public class TaskCreate
    {
        public string Name { get; set; }
        public Project ParentProject { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public List<ApplicationUser> MyProperty { get; set; } = new List<ApplicationUser>();
        public List<HttpPostedFileBase> Files { get; set; } = new List<HttpPostedFileBase>();
    }
}