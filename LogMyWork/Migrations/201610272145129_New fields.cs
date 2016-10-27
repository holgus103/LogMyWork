namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newfields : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserProjectTasks", newName: "ProjectTaskApplicationUsers");
            DropPrimaryKey("dbo.ProjectTaskApplicationUsers");
            AddColumn("dbo.Projects", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "ClientID", c => c.String(maxLength: 128));
            AddColumn("dbo.ProjectTasks", "Status", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProjectTaskApplicationUsers", new[] { "ProjectTask_TaskID", "ApplicationUser_Id" });
            CreateIndex("dbo.Projects", "ClientID");
            AddForeignKey("dbo.Projects", "ClientID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ClientID", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ClientID" });
            DropPrimaryKey("dbo.ProjectTaskApplicationUsers");
            DropColumn("dbo.ProjectTasks", "Status");
            DropColumn("dbo.Projects", "ClientID");
            DropColumn("dbo.Projects", "Status");
            AddPrimaryKey("dbo.ProjectTaskApplicationUsers", new[] { "ApplicationUser_Id", "ProjectTask_TaskID" });
            RenameTable(name: "dbo.ProjectTaskApplicationUsers", newName: "ApplicationUserProjectTasks");
        }
    }
}
