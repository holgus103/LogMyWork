namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyforTasks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectTasks", "ParentProject_ProjectID", "dbo.Projects");
            DropIndex("dbo.ProjectTasks", new[] { "ParentProject_ProjectID" });
            DropColumn("dbo.ProjectTasks", "ParentProjectId");
            RenameColumn(table: "dbo.ProjectTasks", name: "ParentProject_ProjectID", newName: "ParentProjectId");
            AlterColumn("dbo.ProjectTasks", "ParentProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectTasks", "ParentProjectId");
            AddForeignKey("dbo.ProjectTasks", "ParentProjectId", "dbo.Projects", "ProjectID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectTasks", "ParentProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectTasks", new[] { "ParentProjectId" });
            AlterColumn("dbo.ProjectTasks", "ParentProjectId", c => c.Int());
            RenameColumn(table: "dbo.ProjectTasks", name: "ParentProjectId", newName: "ParentProject_ProjectID");
            AddColumn("dbo.ProjectTasks", "ParentProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectTasks", "ParentProject_ProjectID");
            AddForeignKey("dbo.ProjectTasks", "ParentProject_ProjectID", "dbo.Projects", "ProjectID");
        }
    }
}
