using LogMyWork.Consts;
using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LogMyWork.ContextExtensions
{
    public static class ProjectsExtension
    {
        public static IEnumerable<KeyValuePair<object, string>> GetUsersForProjectAsKeyValuePair(this LogMyWorkContext context, int projectID)
        {
            return context.Projects
       .Where(x => x.ProjectID == projectID)
       .Include(x => x.Roles.Select(r => r.User))
       .ToList()
       .SelectMany(x => x.Roles.Select(r => new KeyValuePair<object, string>(r.User.Id, r.User.Email)));
        }

        public static bool HasProjectAccess(this LogMyWorkContext context, int projectID, string userID)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID).Count() > 0;
        }

        public static bool HasProjectRole(this LogMyWorkContext context, int projectID, string userID, Role role)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID && r.Role == role).Count() > 0;
        }

        public static ProjectRole GetUserRoleForProject(this LogMyWorkContext context, int projectID, string userID)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID).FirstOrDefault();
        }
    }
}