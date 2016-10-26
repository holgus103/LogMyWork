namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Project_Roles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "OwnerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Project_ProjectID", "dbo.Projects");
            DropIndex("dbo.Projects", new[] { "OwnerID" });
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_ProjectID" });
            CreateTable(
                "dbo.ApplicationUserProjects",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Project_ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Project_ProjectID })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectID, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Project_ProjectID);
            
            DropColumn("dbo.Projects", "OwnerID");
            DropColumn("dbo.Projects", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "Project_ProjectID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Project_ProjectID", c => c.Int());
            AddColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Projects", "OwnerID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserProjects", "Project_ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ApplicationUserProjects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserProjects", new[] { "Project_ProjectID" });
            DropIndex("dbo.ApplicationUserProjects", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserProjects");
            CreateIndex("dbo.AspNetUsers", "Project_ProjectID");
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            CreateIndex("dbo.Projects", "OwnerID");
            AddForeignKey("dbo.AspNetUsers", "Project_ProjectID", "dbo.Projects", "ProjectID");
            AddForeignKey("dbo.Projects", "OwnerID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
