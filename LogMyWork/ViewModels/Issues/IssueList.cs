using LogMyWork.Consts;
using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Issues
{
    public class IssueList
    {
        public IEnumerable<Issue> Issues { get; set; }
        public Role Role { get; set; }
    }
}