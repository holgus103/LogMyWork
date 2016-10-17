using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.Models
{
    public class TimeEntry
    {
        [Key]
        public int EntryID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Active { get; set; }
        public ProjectTask ParentTask { get; set; }
    }
}
