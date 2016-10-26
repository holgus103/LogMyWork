namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeEntries", "ParentTaskId", "dbo.ProjectTasks");
            DropIndex("dbo.TimeEntries", new[] { "ParentTaskId" });
            CreateTable(
                "dbo.ProjectRoles",
                c => new
                    {
                        ProjecyID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        Project_ProjectID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjecyID, t.UserID })
                .ForeignKey("dbo.Projects", t => t.Project_ProjectID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => new { t.ProjecyID, t.UserID }, unique: true, name: "ProjectRole_Index")
                .Index(t => t.Project_ProjectID)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.TimeEntries", "ParentTask_TaskID", c => c.Int());
            CreateIndex("dbo.TimeEntries", "ParentTask_TaskID");
            AddForeignKey("dbo.TimeEntries", "ParentTask_TaskID", "dbo.ProjectTasks", "TaskID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "ParentTask_TaskID", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectRoles", "Project_ProjectID", "dbo.Projects");
            DropIndex("dbo.TimeEntries", new[] { "ParentTask_TaskID" });
            DropIndex("dbo.ProjectRoles", new[] { "User_Id" });
            DropIndex("dbo.ProjectRoles", new[] { "Project_ProjectID" });
            DropIndex("dbo.ProjectRoles", "ProjectRole_Index");
            DropColumn("dbo.TimeEntries", "ParentTask_TaskID");
            DropTable("dbo.ProjectRoles");
            CreateIndex("dbo.TimeEntries", "ParentTaskId");
            AddForeignKey("dbo.TimeEntries", "ParentTaskId", "dbo.ProjectTasks", "TaskID", cascadeDelete: true);
        }
    }
}
