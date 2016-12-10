using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class Issue
    {
        public int IssueID { get; set; }
        public int IssueNumber { get; set; }
        public Project Project { get; set; }
        public int ProjectID { get; set; }
        public string Description { get; set; }
        public ApplicationUser Reporter { get; set; }
        public int ReporterID { get; set; }
        public List<Attachment> Attachments { get; set; }
        public DateTime RaportDate { get; set; }
        public DateTime LastModified { get; set; }
        public IssueStatus Status { get; set; }
        public ProjectTask Task { get; set; }
        public int? TaskID { get; set; }
    }
}