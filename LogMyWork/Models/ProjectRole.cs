using LogMyWork.Consts;
using System.ComponentModel.DataAnnotations;

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