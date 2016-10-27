namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatestoProjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RateProjects",
                c => new
                    {
                        Rate_RateID = c.Int(nullable: false),
                        Project_ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Rate_RateID, t.Project_ProjectID })
                .ForeignKey("dbo.Rates", t => t.Rate_RateID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectID, cascadeDelete: true)
                .Index(t => t.Rate_RateID)
                .Index(t => t.Project_ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateProjects", "Project_ProjectID", "dbo.Projects");
            DropForeignKey("dbo.RateProjects", "Rate_RateID", "dbo.Rates");
            DropIndex("dbo.RateProjects", new[] { "Project_ProjectID" });
            DropIndex("dbo.RateProjects", new[] { "Rate_RateID" });
            DropTable("dbo.RateProjects");
        }
    }
}
