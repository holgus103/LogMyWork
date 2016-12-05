using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using LogMyWork.Consts;

namespace LogMyWork.ContextExtensions
{
    public static class UsersExtension
    {
        public static IEnumerable<KeyValuePair<object, string>> GetUsersAsKeyValuePair(this LogMyWorkContext context)
        {
            return context.Users
                .ToList()
                .Select(u => new KeyValuePair<object, string>(u.Id, u.Email));
        }

        public static IEnumerable<KeyValuePair<object, string>> GetUsersForProjectAsKeyValuePair(this LogMyWorkContext context, int projectID)
        {
            return context.Projects
       .Where(x => x.ProjectID == projectID)
       .Include(x => x.Roles.Select(r => r.User))
       .ToList()
       .SelectMany(x => x.Roles.Select(r => new KeyValuePair<object, string>(r.User.Id, r.User.Email)));
        }

        public static IQueryable<ApplicationUser> GetRelatedUsers(this LogMyWorkContext context, string userID)
        {
            return context.ProjectRoles.Where(r => r.UserID == userID && r.Role == Role.Owner)
                .Include(r => r.Project.Roles.Select(r2 => r2.User))
                .SelectMany(r => r.Project.Roles.Select(r2 => r2.User))
                .Union(
                    context.Users.Where(u => u.Id == userID)
                    .Include(u => u.OwnedTasks.Select(t => t.Users))
                    .SelectMany(u => u.OwnedTasks.SelectMany(t => t.Users))
                )
                .Union(
                    context.Users.Where(u => u.Id == userID)
                );
        }
    }
}