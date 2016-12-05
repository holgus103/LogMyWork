using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.DTO.Filters
{
    public class FilterCreateDTO
    {
        public int ProjectID { get; set; }
        public int TaskID { get; set; }
        public string UserID  { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public FilterType FilterType { get; set; }
    }
}