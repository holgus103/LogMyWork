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
            RenameColumn(table: "dbo.TimeEntries", name: "ParentTask_TaskID", newName: "ParentTaskId");
            AlterColumn("dbo.TimeEntries", "ParentTaskId", c => c.Int(nullable: false));
            CreateIndex("dbo.TimeEntries", "ParentTaskId");
            AddForeignKey("dbo.TimeEntries", "ParentTaskId", "dbo.ProjectTasks", "TaskID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "ParentTaskId", "dbo.ProjectTasks");
            DropIndex("dbo.TimeEntries", new[] { "ParentTaskId" });
            AlterColumn("dbo.TimeEntries", "ParentTaskId", c => c.Int());
            RenameColumn(table: "dbo.TimeEntries", name: "ParentTaskId", newName: "ParentTask_TaskID");
            CreateIndex("dbo.TimeEntries", "ParentTask_TaskID");
            AddForeignKey("dbo.TimeEntries", "ParentTask_TaskID", "dbo.ProjectTasks", "TaskID");
        }
    }
}
