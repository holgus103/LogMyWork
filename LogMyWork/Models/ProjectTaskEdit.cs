using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class ProjectTaskEdit
    {
        public string Name { get; set; }
        public int ParentProjectID { get; set; }
        public List<int> Users { get; set; } = new List<int>();
    }
}