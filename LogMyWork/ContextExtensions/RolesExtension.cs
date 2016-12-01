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

        public static bool isProjectOwner(this LogMyWorkContext context, int projectID, string userID)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID && r.Role == Role.Owner).Count() > 0;
        }
    }
}