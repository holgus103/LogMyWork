using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.DTO.Projects
{
    public class ProjectCreateDTO
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public Rate Rate { get; set; }
    }
}