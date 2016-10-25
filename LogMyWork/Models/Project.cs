using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.Models
{
    [Table("Projects")]
    public class Project
    {
        [ForeignKey("User")]
        public string OwnerID { get; }
        public ApplicationUser Owner { get; }
        public int ProjectID { get; set; }
        //[Required]
        [DisplayName("Project Name")]
        public string Name { get; set; }
        public IEnumerable<ProjectTask> Tasks{ get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
