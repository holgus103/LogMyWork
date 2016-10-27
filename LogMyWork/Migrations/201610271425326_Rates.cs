namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        RateID = c.Int(nullable: false, identity: true),
                        RateValue = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RateID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.TimeEntries", "Billed", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.TimeEntries", "RateID", c => c.Int(nullable: true));
            CreateIndex("dbo.TimeEntries", "RateID");
            AddForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates", "RateID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "RateID", "dbo.Rates");
            DropForeignKey("dbo.Rates", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Rates", new[] { "User_Id" });
            DropIndex("dbo.TimeEntries", new[] { "RateID" });
            DropColumn("dbo.TimeEntries", "RateID");
            DropColumn("dbo.TimeEntries", "Billed");
            DropTable("dbo.Rates");
        }
    }
}
