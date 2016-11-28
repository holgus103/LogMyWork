using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ViewModels.Tasks
{
    public class TaskCreate : TaskCreateDTO
    {
        public List<KeyValuePair<object, string>> SelectableProjects { get; set; }
        public List<KeyValuePair<object, string>> SelectableUsers { get; set; }
        public List<Attachment> Attachments { get; set; }

    }
}