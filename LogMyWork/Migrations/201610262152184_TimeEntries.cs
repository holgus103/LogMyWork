namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeEntries : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeEntries", "ParentTask_TaskID", "dbo.ProjectTasks");
            DropIndex("dbo.TimeEntries", new[] { "ParentTask_TaskID" });
            DropColumn("dbo.TimeEntries", "ParentTaskID");
            RenameColumn(table: "dbo.TimeEntries", name: "ParentTask_TaskID", newName: "ParentTaskID");
            AlterColumn("dbo.TimeEntries", "ParentTaskID", c => c.Int(nullable: false));
            CreateIndex("dbo.TimeEntries", "ParentTaskID");
            AddForeignKey("dbo.TimeEntries", "ParentTaskID", "dbo.ProjectTasks", "TaskID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "ParentTaskID", "dbo.ProjectTasks");
            DropIndex("dbo.TimeEntries", new[] { "ParentTaskID" });
            AlterColumn("dbo.TimeEntries", "ParentTaskID", c => c.Int());
            RenameColumn(table: "dbo.TimeEntries", name: "ParentTaskID", newName: "ParentTask_TaskID");
            AddColumn("dbo.TimeEntries", "ParentTaskID", c => c.Int(nullable: false));
            CreateIndex("dbo.TimeEntries", "ParentTask_TaskID");
            AddForeignKey("dbo.TimeEntries", "ParentTask_TaskID", "dbo.ProjectTasks", "TaskID");
        }
    }
}
