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
        public int ParentProjectID { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public IEnumerable<HttpPostedFileBase> Files { get; set; } 
    }
}