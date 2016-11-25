using LogMyWork.Models;
using System.Collections.Generic;

namespace LogMyWork.ViewModels.TimeEntries
{
    public class TimeEntryIndex
    {
        public List<TimeEntry> TimeEntries { get; set; }
        public List<Project> Projects { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
