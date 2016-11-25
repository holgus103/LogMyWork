using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Tasks
{
    public class TaskAdd
    {
        public string Name { get; set; }
        public int ParentProjectID { get; set; }
        public List<ProjectRole> Roles { get; set; }
        public List<ApplicationUser> Users { get; set; }

    }
}