namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdinTimeEntries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeEntries", "UserID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeEntries", "UserID");
        }
    }
}
