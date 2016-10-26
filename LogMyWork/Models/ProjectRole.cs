using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class ProjectRole
    {
        [Key]
        public int ProjectRoleID { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }
}