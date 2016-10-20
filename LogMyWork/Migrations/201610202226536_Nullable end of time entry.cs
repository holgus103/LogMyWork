namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nullableendoftimeentry : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeEntries", "End", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeEntries", "End", c => c.DateTime(nullable: false));
        }
    }
}
