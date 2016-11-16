using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogMyWork.Models
{
    [Table("Projects")]
    public class Project
    {
        public int ProjectID { get; set; }
        [DisplayName("Project Name")]
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public string ClientID { get; set; }
        [ForeignKey("ClientID")]
        public ApplicationUser Client { get; set; }
        public List<ProjectTask> Tasks{ get; set; }
        public List<ProjectRole> Roles { get; set; }
        public List<Rate> Rates { get; set; }
    }
}
