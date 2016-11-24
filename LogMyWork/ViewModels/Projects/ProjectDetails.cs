using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Projects
{
    public class ProjectDetails
    {
        public Project Project { get; set; }
        public ProjectRole CurrentProjectRole { get; set; }
    }
}