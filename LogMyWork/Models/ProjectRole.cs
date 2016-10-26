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
        private Project project;
        private ApplicationUser user;

        //[Key]
        //public int ProjectRoleID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ProjectID { get; set; }
        public Project Project
        {
            get
            {
                return this.project;
            }
            set
            {
                this.project = value;
                this.ProjectID = value.ProjectID;
            }
        }
        [Key]
        [Column(Order = 2)]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }
}