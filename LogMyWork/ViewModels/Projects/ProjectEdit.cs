using LogMyWork.Models;

namespace LogMyWork.ViewModels.Projects
{
    public class ProjectEdit
    {
        public int? ProjectID { get; set; }
        public string Name { get; set; }
        public int RateID { get; set; }
        public ProjectRole Role {get; set;}
    }
}