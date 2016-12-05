using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

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

    }
}