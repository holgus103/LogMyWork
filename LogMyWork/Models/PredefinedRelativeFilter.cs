using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.Models
{
    public class PredefinedRelativeFilter : PredefinedFilterBase
    {
        public FilterType FilterType { get; set; }
        public int Amount { get; set; }
        public TimeUnitEnum Unit { get; set; }
    }
}
