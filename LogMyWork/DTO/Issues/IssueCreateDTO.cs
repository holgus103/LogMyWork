using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogMyWork.DTO.Issues
{
    public class IssueCreateDTO
    {
        public int IssueID { get; set; }
        [Range(1,int.MaxValue)]
        public int ProjectID { get; set; }
        [Required]
        public string Title { get; set; } 
        public string Description { get; set; }

    }
}