using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogMyWork.Models
{
    public class ProjectEdit
    {
        public int? ProjectID { get; set; }
        public string Name { get; set; }
        public int RateID { get; set; }
        public ProjectRole Role {get; set;}
    }
}