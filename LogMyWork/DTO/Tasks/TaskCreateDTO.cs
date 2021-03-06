﻿using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Tasks
{
    public class TaskCreateDTO
    {
        public int TaskID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Project")]
        public int ParentProjectID { get; set; }
        public string Description { get; set; }
        public ulong Deadline { get; set; }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public IEnumerable<HttpPostedFileBase> Files { get; set; } 
    }
}