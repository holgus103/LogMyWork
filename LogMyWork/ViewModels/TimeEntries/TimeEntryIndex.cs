using LogMyWork.Models;
using System;
using System.Collections.Generic;

namespace LogMyWork.ViewModels.TimeEntries
{
    public class TimeEntryIndex
    {

        public List<Project> Projects { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public TimeEntriesTable TimeEntries { get; set; } = new TimeEntriesTable();
        public List<PredefinedStaticFilter> StaticFilters { get; set; }
        //public List<PredefinedRelativeFilter> RelativeFilters { get; set; }

    }
}
