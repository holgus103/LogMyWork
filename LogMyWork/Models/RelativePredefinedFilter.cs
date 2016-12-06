using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.Models
{
    class RelativePredefinedFilter : PredefinedFilterBase
    {
        public FilterType FilterType { get; set; }
        public int Amount { get; set; }
        public TimeUnitEnum Unit { get; set; }
    }
}
