using LogMyWork.Consts;
using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.ContextExtensions
{
    public static class RolesExtension
    {
        public static bool HasProjectRole(this LogMyWorkContext context, int projectID, string userID, Role role)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID && r.Role <= role).Count() > 0;
        }

        public static ProjectRole GetUserRoleForProject(this LogMyWorkContext context, int projectID, string userID)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID).FirstOrDefault();
        }

    }
}