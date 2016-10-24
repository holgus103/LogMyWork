namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdisastring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeEntries", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeEntries", "UserID", c => c.Int(nullable: false));
        }
    }
}
