using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.DTO.Filters
{
    public class StaticFilterCreateDTO
    {
        public int FilterID { get; set; }
        public int? ProjectID { get; set; }
        public int? TaskID { get; set; }
        public string UserID  { get; set; }
        public ulong From { get; set; }
        public ulong To { get; set; }
    }
}