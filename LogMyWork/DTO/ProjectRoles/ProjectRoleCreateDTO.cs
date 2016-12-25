using Commons.Web.Attributes;
using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.DTO.ProjectRoles
{
    public class ProjectRoleCreateDTO
    {
        [Range(1, int.MaxValue)]
        public int ProjectID { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string UserID { get; set; }
    }
}
