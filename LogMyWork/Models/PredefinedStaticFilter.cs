using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class PredefinedStaticFilter : PredefinedFilterBase
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}