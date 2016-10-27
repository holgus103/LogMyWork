namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableRatesforTimeEntries : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates");
            DropIndex("dbo.TimeEntries", new[] { "RateID" });
            AlterColumn("dbo.TimeEntries", "RateID", c => c.Int());
            CreateIndex("dbo.TimeEntries", "RateID");
            AddForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates", "RateID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates");
            DropIndex("dbo.TimeEntries", new[] { "RateID" });
            AlterColumn("dbo.TimeEntries", "RateID", c => c.Int(nullable: false));
            CreateIndex("dbo.TimeEntries", "RateID");
            AddForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates", "RateID", cascadeDelete: true);
        }
    }
}
