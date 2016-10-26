namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Somekeysremoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserProjects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserProjects", "Project_ProjectID", "dbo.Projects");
            DropIndex("dbo.ApplicationUserProjects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserProjects", new[] { "Project_ProjectID" });
            DropTable("dbo.ApplicationUserProjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserProjects",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Project_ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Project_ProjectID });
            
            CreateIndex("dbo.ApplicationUserProjects", "Project_ProjectID");
            CreateIndex("dbo.ApplicationUserProjects", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserProjects", "Project_ProjectID", "dbo.Projects", "ProjectID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserProjects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
