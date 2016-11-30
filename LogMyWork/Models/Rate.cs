using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogMyWork.Models
{
    public class Rate
    {

        public class IdComparer : IEqualityComparer<Rate>
        {
            public bool Equals(Rate x, Rate y)
            {
                return x.RateID == y.RateID;
            }

            public int GetHashCode(Rate obj)
            {
                return obj.GetHashCode();
            }
        }

        [Key]
        public int RateID { get; set; }
        public double RateValue { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Project> Projects { get; set; }

    }
}