using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogMyWork.Models
{
    [Table("ProjectTasks")]
    public class ProjectTask
    {
        [Key]
        public int TaskID { get; set; }
        public ProjectStatus Status { get; set; }
        public List<ApplicationUser> Users { get; set; } 
        [DisplayName ("Task Name")]
        public string Name { get; set; }
        [ForeignKey ("ParentProject")]
        public int ParentProjectID { get; set; }
        public Project ParentProject { get; set; }
    }
}