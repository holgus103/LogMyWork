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

        public static bool HasTaskAccess(this LogMyWorkContext context, int taskID, string userID)
        {
            return context.ProjectTasks.Where(t => t.TaskID == taskID)
                .Include(t => t.ParentProject.Roles)
                .Include(t => t.Users)
                .Where(t => t.OwnerID == userID || t.Users.Any(u => u.Id == userID) || t.ParentProject.Roles.Any(r => r.Role == Role.Owner && r.UserID == userID))
                .Count() > 0;
        }
    }
}