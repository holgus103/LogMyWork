namespace LogMyWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RateUserId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rates", new[] { "User_Id" });
            DropColumn("dbo.Rates", "UserId");
            RenameColumn(table: "dbo.Rates", name: "User_Id", newName: "UserID");
            AlterColumn("dbo.Rates", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rates", "UserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rates", new[] { "UserID" });
            AlterColumn("dbo.Rates", "UserID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Rates", name: "UserID", newName: "User_Id");
            AddColumn("dbo.Rates", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Rates", "User_Id");
        }
    }
}
