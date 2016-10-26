namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typo : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ProjectRoles");
            DropForeignKey("dbo.ProjectRoles", "Project_ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectRoles", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectRoles", "ProjectRole_Index");
            DropIndex("dbo.ProjectRoles", new[] { "Project_ProjectID" });
            DropIndex("dbo.ProjectRoles", new[] { "User_Id" });
            DropColumn("dbo.ProjectRoles", "UserID");
            RenameColumn(table: "dbo.ProjectRoles", name: "Project_ProjectID", newName: "ProjectID");
            RenameColumn(table: "dbo.ProjectRoles", name: "User_Id", newName: "UserID");
            AlterColumn("dbo.ProjectRoles", "UserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ProjectRoles", "ProjectID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectRoles", "UserID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ProjectRoles", new[] { "ProjectID", "UserID" });
            CreateIndex("dbo.ProjectRoles", "ProjectID");
            CreateIndex("dbo.ProjectRoles", "UserID");
            AddForeignKey("dbo.ProjectRoles", "ProjectID", "dbo.Projects", "ProjectID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.ProjectRoles", "ProjecyID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectRoles", "ProjecyID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProjectRoles", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectRoles", "ProjectID", "dbo.Projects");
            DropIndex("dbo.ProjectRoles", new[] { "UserID" });
            DropIndex("dbo.ProjectRoles", new[] { "ProjectID" });
            DropPrimaryKey("dbo.ProjectRoles");
            AlterColumn("dbo.ProjectRoles", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.ProjectRoles", "ProjectID", c => c.Int());
            AlterColumn("dbo.ProjectRoles", "UserID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProjectRoles", new[] { "ProjecyID", "UserID" });
            RenameColumn(table: "dbo.ProjectRoles", name: "UserID", newName: "User_Id");
            RenameColumn(table: "dbo.ProjectRoles", name: "ProjectID", newName: "Project_ProjectID");
            AddColumn("dbo.ProjectRoles", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectRoles", "User_Id");
            CreateIndex("dbo.ProjectRoles", "Project_ProjectID");
            CreateIndex("dbo.ProjectRoles", new[] { "ProjecyID", "UserID" }, unique: true, name: "ProjectRole_Index");
            AddForeignKey("dbo.ProjectRoles", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProjectRoles", "Project_ProjectID", "dbo.Projects", "ProjectID");
        }
    }
}
