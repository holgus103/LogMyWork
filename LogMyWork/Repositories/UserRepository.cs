using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Repositories
{
    public static class UserRepository
    {
        public static IEnumerable<KeyValuePair<object, string>> GetUsersAsKeyValuePair(this LogMyWorkContext context)
        {
            return context.Users
                .ToList()
                .Select(u => new KeyValuePair<object, string>(u.Id, u.Email));
        }
    }
}