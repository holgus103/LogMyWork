using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.TimeEntries
{
    public class TimeEntriesTable
    {
        public IEnumerable<TimeEntry> TimeEntries { get; set; }
        public TimeSpan Sum { get; set; }
        public double TotalEarned { get; set; }
    }
}