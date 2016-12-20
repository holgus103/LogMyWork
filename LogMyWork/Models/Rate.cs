using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LogMyWork.Models
{
    public class Rate
    {
        [Key]
        public int RateID { get; set; }
        [DisplayName("Rate value")]
        public double RateValue { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Project> Projects { get; set; }

    }
}