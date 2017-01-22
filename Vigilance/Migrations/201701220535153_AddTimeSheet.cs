namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeSheet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.Int(nullable: false),
                        LogOnTime = c.DateTime(nullable: false),
                        LogOnLatLng_Lat = c.Single(),
                        LogOnLatLng_Lng = c.Single(),
                        LogOffTime = c.DateTime(),
                        LogOffLatLng_Lat = c.Single(),
                        LogOffLatLng_Lng = c.Single(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        WorkSite_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.WorkSites", t => t.WorkSite_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.WorkSite_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSheets", "WorkSite_Id", "dbo.WorkSites");
            DropForeignKey("dbo.TimeSheets", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TimeSheets", new[] { "WorkSite_Id" });
            DropIndex("dbo.TimeSheets", new[] { "ApplicationUser_Id" });
            DropTable("dbo.TimeSheets");
        }
    }
}
