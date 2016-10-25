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
        [ForeignKey("User")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        [DisplayName ("Task Name")]
        public string Name { get; set; }
        [ForeignKey ("ParentProject")]
        public int ParentProjectId { get; set; }
        public Project ParentProject { get; set; }
    }
}