using LogMyWork.Consts;
using System;
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
        [ForeignKey("Owner")]
        public string OwnerID { get; set; }
        public ApplicationUser Owner { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<TimeEntry> Entries { get; set; }
        [DisplayName("Task Name")]
        public string Name { get; set; }
        [ForeignKey("ParentProject")]
        public int ParentProjectID { get; set; }
        public Project ParentProject { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public List<Attachment> Attachments { get; set; }

        public ProjectTask()
        {
            this.LastModified = DateTime.UtcNow;

        }
    }
}