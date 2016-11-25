using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.ViewModels.TimeEntries
{
    public class TimeEntryIndex
    {
        public List<TimeEntry> TimeEntries { get; set; }
        public List<ProjectRole> Roles { get; set; }
    }
}
