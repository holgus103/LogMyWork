namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedsomeid : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Rates", "ProjectID");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Rates", "ProjectID", c => c.Int(nullable: false));
        }
    }
}
