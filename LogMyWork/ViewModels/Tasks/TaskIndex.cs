using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Tasks
{
    public class TaskIndex
    {
        public IEnumerable<ProjectTask> OwnedTasks { get; set; }
        public IEnumerable<ProjectTask> AssignedTasks { get; set; }
    }
}