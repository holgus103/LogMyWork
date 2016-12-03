using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Projects
{
    public class ProjectUsers
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<ProjectRole> Users { get; set; }
        public ProjectRole CurrentUserRole { get; set; }
    }
}