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
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool Active { get; set; }
        public int ParentTaskID { get; set; }
        public ProjectTask ParentTask { get; set; }
        [DisplayFormat(DataFormatString = @"{0:hh\:mm\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? Duration
        {
            get
            {
                if (this.End != null)
                    return this.End.Value.Subtract(this.Start);
                else
                    return null;
            }
        }
    }
}
