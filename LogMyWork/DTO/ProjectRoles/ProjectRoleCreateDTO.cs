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
        public int ProjectID { get; set; }
        //[DisallowEnumValueAttribute(typeof(Role), new SortedSet<object>() { Role.Owner })]
        public Role Role { get; set; }
        public string UserID { get; set; }
    }
}
