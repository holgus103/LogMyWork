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


        public static bool HasProjectAccess(this LogMyWorkContext context, int projectID, string userID)
        {
            return context.ProjectRoles.Where(r => r.ProjectID == projectID && r.UserID == userID).Count() > 0;
        }



        public static IQueryable<Project> GetProjectsForUser(this LogMyWorkContext context, string userID)
        {
            return context.ProjectRoles
                .Where(r => r.UserID == userID)
                .Include(r => r.Project)
                .Select(r => r.Project);
        }
    }
}