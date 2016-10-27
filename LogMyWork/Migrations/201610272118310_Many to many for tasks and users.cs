namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Manytomanyfortasksandusers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectTasks", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectTasks", new[] { "UserID" });
            CreateTable(
                "dbo.ApplicationUserProjectTasks",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        ProjectTask_TaskID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.ProjectTask_TaskID })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProjectTasks", t => t.ProjectTask_TaskID, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ProjectTask_TaskID);
            
            DropColumn("dbo.ProjectTasks", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectTasks", "UserID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserProjectTasks", "ProjectTask_TaskID", "dbo.ProjectTasks");
            DropForeignKey("dbo.ApplicationUserProjectTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserProjectTasks", new[] { "ProjectTask_TaskID" });
            DropIndex("dbo.ApplicationUserProjectTasks", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserProjectTasks");
            CreateIndex("dbo.ProjectTasks", "UserID");
            AddForeignKey("dbo.ProjectTasks", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
