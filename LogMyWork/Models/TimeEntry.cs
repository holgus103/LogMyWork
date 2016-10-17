using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey ("ParentTask")]
        public int ParentTaskId { get; set; }
        public ProjectTask ParentTask { get; set; }
    }
}
