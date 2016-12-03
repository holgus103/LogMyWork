using LogMyWork.Consts;
using LogMyWork.DTO.ProjectRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.ViewModels.ProjectRoles
{
    public class ProjectRoleCreate : ProjectRoleCreateDTO
    {
        public IEnumerable<KeyValuePair<object, string>> SelectableUsers { get; set; }
        public IEnumerable<Role> SelectableRoles { get; set; }
    }
}
