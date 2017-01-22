namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserIdToStringInTimeSheet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeSheets", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TimeSheets", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.TimeSheets", "ApplicationUserId", c => c.String(nullable: false));
            DropColumn("dbo.TimeSheets", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TimeSheets", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.TimeSheets", "ApplicationUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.TimeSheets", "ApplicationUser_Id");
            AddForeignKey("dbo.TimeSheets", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
