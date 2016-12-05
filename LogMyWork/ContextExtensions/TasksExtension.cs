using LogMyWork.Consts;
using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace LogMyWork.ContextExtensions
{
    public static class TasksExtension
    {
        public static IQueryable<ProjectTask> GetAllTasksForUser(this LogMyWorkContext context, string userID)
        {
            return context.ProjectRoles.Where(r => r.UserID == userID && r.Role == Role.Owner)
                .Include(r => r.Project.Tasks)
                .SelectMany(r => r.Project.Tasks)
                .Union(
                    context.Users.Where(u => u.Id == userID)
                    .Include(u => u.OwnedTasks)
                    .Include(u => u.Tasks)
                    .SelectMany(u => u.OwnedTasks.Union(u.Tasks))
                );
        }
    }
}