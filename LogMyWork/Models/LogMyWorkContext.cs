﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace LogMyWork.Models
{
    public class LogMyWorkContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public LogMyWorkContext() : base("name=LogMyWorkContext", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static LogMyWorkContext Create()
        {
            return new LogMyWorkContext();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTask> ProjectTasks { get; set; }

        public DbSet<TimeEntry> TimeEntries { get; set; }

        public DbSet<ProjectRole> ProjectRoles { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<PredefinedStaticFilter> StaticFilters { get; set; }

        //public DbSet<PredefinedRelativeFilter> RelativeFilters { get; set; }

        public System.Data.Entity.DbSet<LogMyWork.Models.Issue> Issues { get; set; }
    }
}
