using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class PredefinedFilterBase
    {
        [Key]
        public int FilterID { get; set; }
        public ApplicationUser Owner { get; set; }
        public string OwnerID { get; set; }
        public int TaskID { get; set; }
        public int ProjectID { get; set; }
        public string UserID { get; set; }
    }
}