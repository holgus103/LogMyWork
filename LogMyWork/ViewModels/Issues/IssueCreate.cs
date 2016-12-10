using LogMyWork.DTO.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Issues
{
    public class IssueCreate : IssueCreateDTO
    {
        public IEnumerable<KeyValuePair<object, string>> SelectableProjects { get; set; }
    }
}