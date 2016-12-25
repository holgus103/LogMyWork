using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        //public bool Billed { get; set; }
        //public int? RateID { get; set; }
        //public Rate  Rate { get; set; }
        public int ParentTaskID { get; set; }
        [ForeignKey("ParentTaskID")]
        public ProjectTask ParentTask { get; set; }
        public String DuractionString
        {
            get
            {
                TimeSpan? span = this.Duration;
                if (!span.HasValue)
                {
                    return "0:00:00";
                }
                return String.Format("{0:D2}:{1:D2}:{2:D2}", (int)span.Value.TotalHours, span.Value.Minutes, span.Value.Seconds);
            }
        }
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

        public double Charge
        {
            get
            {
                double? rate = this.ParentTask?.ParentProject?.Rates?.Where(r => r.UserID == UserID).FirstOrDefault()?.RateValue;
                if (rate.HasValue && this.Duration.HasValue)
                    return rate.Value / 60 * this.Duration.Value.TotalMinutes;
                else
                    return 0;
            }
        }
    }
}
