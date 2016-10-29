namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectRoles",
                c => new
                    {
                        ProjectRoleID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectRoleID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.ProjectID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        ClientID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.AspNetUsers", t => t.ClientID)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Name = c.String(),
                        ParentProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.Projects", t => t.ParentProjectID, cascadeDelete: true)
                .Index(t => t.ParentProjectID);
            
            CreateTable(
                "dbo.TimeEntries",
                c => new
                    {
                        EntryID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        Billed = c.Boolean(nullable: false),
                        RateID = c.Int(),
                        ParentTaskID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EntryID)
                .ForeignKey("dbo.ProjectTasks", t => t.ParentTaskID, cascadeDelete: true)
                .ForeignKey("dbo.Rates", t => t.RateID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.RateID)
                .Index(t => t.ParentTaskID);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        RateID = c.Int(nullable: false, identity: true),
                        RateValue = c.Double(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RateID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProjectTaskApplicationUsers",
                c => new
                    {
                        ProjectTask_TaskID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectTask_TaskID, t.ApplicationUser_Id })
                .ForeignKey("dbo.ProjectTasks", t => t.ProjectTask_TaskID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.ProjectTask_TaskID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.RateProjects",
                c => new
                    {
                        Rate_RateID = c.Int(nullable: false),
                        Project_ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Rate_RateID, t.Project_ProjectID })
                .ForeignKey("dbo.Rates", t => t.Rate_RateID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectID, cascadeDelete: true)
                .Index(t => t.Rate_RateID)
                .Index(t => t.Project_ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectRoles", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.TimeEntries", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates");
            DropForeignKey("dbo.Rates", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.RateProjects", "Project_ProjectID", "dbo.Projects");
            DropForeignKey("dbo.RateProjects", "Rate_RateID", "dbo.Rates");
            DropForeignKey("dbo.TimeEntries", "ParentTaskID", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectTaskApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectTaskApplicationUsers", "ProjectTask_TaskID", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectTasks", "ParentProjectID", "dbo.Projects");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "ClientID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.RateProjects", new[] { "Project_ProjectID" });
            DropIndex("dbo.RateProjects", new[] { "Rate_RateID" });
            DropIndex("dbo.ProjectTaskApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectTaskApplicationUsers", new[] { "ProjectTask_TaskID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Rates", new[] { "UserID" });
            DropIndex("dbo.TimeEntries", new[] { "ParentTaskID" });
            DropIndex("dbo.TimeEntries", new[] { "RateID" });
            DropIndex("dbo.TimeEntries", new[] { "UserID" });
            DropIndex("dbo.ProjectTasks", new[] { "ParentProjectID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Projects", new[] { "ClientID" });
            DropIndex("dbo.ProjectRoles", new[] { "UserID" });
            DropIndex("dbo.ProjectRoles", new[] { "ProjectID" });
            DropTable("dbo.RateProjects");
            DropTable("dbo.ProjectTaskApplicationUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Rates");
            DropTable("dbo.TimeEntries");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectRoles");
        }
    }
}
