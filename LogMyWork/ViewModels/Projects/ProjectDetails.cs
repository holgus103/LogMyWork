using LogMyWork.Models;
using LogMyWork.ViewModels.Tasks;
using System.Collections.Generic;

namespace LogMyWork.ViewModels.Projects
{
    public class ProjectDetails
    {
        public Project Project { get; set; }
        public ProjectRole CurrentProjectRole { get; set; }
        public TaskIndex Tasks { get; set; }
    }
}