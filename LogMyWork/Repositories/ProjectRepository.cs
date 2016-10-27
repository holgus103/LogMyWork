using LogMyWork.Models;
using System.Linq;

namespace LogMyWork.Repositories
{
    public class ProjectRepository  : Repository
    {
        public ProjectRepository(LogMyWorkContext ctx) : base(ctx)
        {

        }

        public IQueryable<Project> GetAllProjectsForUser(string userID)
        {
            return (from p in this.parent.Projects
             join r in this.parent.ProjectRoles on p.ProjectID equals r.ProjectID
             where r.UserID == userID
             select p);
        }
    }
}