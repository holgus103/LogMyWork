using LogMyWork.DTO.Projects;
using LogMyWork.Models;
using System.Collections.Generic;

namespace LogMyWork.ViewModels.Projects
{
    public class ProjectCreate : ProjectCreateDTO
    {
        public IEnumerable<KeyValuePair<object, string>> UserRates { get; set; }
        public ProjectRole Role { get; set; }
        public IEnumerable<KeyValuePair<object, string>> Users { get; set; }

    }
}