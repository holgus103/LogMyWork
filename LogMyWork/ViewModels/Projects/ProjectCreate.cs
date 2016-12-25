using LogMyWork.DTO.Projects;
using LogMyWork.Models;
using LogMyWork.ViewModels.Tasks;
using System.Collections.Generic;

namespace LogMyWork.ViewModels.Projects
{
    public class ProjectCreate : ProjectCreateDTO
    {
        public IEnumerable<KeyValuePair<object, string>> UserRates { get; set; }

    }
}