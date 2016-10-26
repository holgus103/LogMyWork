namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdforProjectRole : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectRoles", new[] { "UserID" });
            DropPrimaryKey("dbo.ProjectRoles");
            AddColumn("dbo.ProjectRoles", "ProjectRoleID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ProjectRoles", "UserID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.ProjectRoles", "ProjectRoleID");
            CreateIndex("dbo.ProjectRoles", "UserID");
            AddForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectRoles", new[] { "UserID" });
            DropPrimaryKey("dbo.ProjectRoles");
            AlterColumn("dbo.ProjectRoles", "UserID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.ProjectRoles", "ProjectRoleID");
            AddPrimaryKey("dbo.ProjectRoles", new[] { "ProjectID", "UserID" });
            CreateIndex("dbo.ProjectRoles", "UserID");
            AddForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
