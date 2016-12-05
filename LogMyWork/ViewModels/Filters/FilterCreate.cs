using LogMyWork.DTO.Filters;
using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMyWork.ViewModels.Filters
{
    public class FilterCreate : FilterCreateDTO
    {
        public IEnumerable<KeyValuePair<object, string>> SelectableProjects { get; set; }

    }
}
