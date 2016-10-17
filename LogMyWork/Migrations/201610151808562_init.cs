namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentProjectId = c.Int(nullable: false),
                        ParentProject_ProjectID = c.Int(),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.Projects", t => t.ParentProject_ProjectID)
                .Index(t => t.ParentProject_ProjectID);
            
            CreateTable(
                "dbo.TimeEntries",
                c => new
                    {
                        EntryID = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ParentTask_TaskID = c.Int(),
                    })
                .PrimaryKey(t => t.EntryID)
                .ForeignKey("dbo.ProjectTasks", t => t.ParentTask_TaskID)
                .Index(t => t.ParentTask_TaskID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "ParentTask_TaskID", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectTasks", "ParentProject_ProjectID", "dbo.Projects");
            DropIndex("dbo.TimeEntries", new[] { "ParentTask_TaskID" });
            DropIndex("dbo.ProjectTasks", new[] { "ParentProject_ProjectID" });
            DropTable("dbo.TimeEntries");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Projects");
        }
    }
}
