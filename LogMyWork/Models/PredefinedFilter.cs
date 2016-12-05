using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class PredefinedFilter
    {
        [Key]
        public int FilterID { get; set; }
        public ApplicationUser Owner { get; set; }
        public string OwnerID { get; set; }
        public int TaskID { get; set; }
        public int ProjectID { get; set; }
        public int UserID { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public FilterType FilterType { get; set; }

    }
}