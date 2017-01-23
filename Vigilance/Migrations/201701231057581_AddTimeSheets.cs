namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeSheets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        SiteName = c.String(nullable: false),
                        SiteAddress = c.String(),
                        LogOnTime = c.DateTime(nullable: false),
                        LogOffTime = c.DateTime(),
                        LogOffLocation_Id = c.Int(),
                        LogOnLocation_Id = c.Int(),
                        SiteLocation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.LatLngs", t => t.LogOffLocation_Id)
                .ForeignKey("dbo.LatLngs", t => t.LogOnLocation_Id)
                .ForeignKey("dbo.LatLngs", t => t.SiteLocation_Id)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.LogOffLocation_Id)
                .Index(t => t.LogOnLocation_Id)
                .Index(t => t.SiteLocation_Id);
            
            CreateTable(
                "dbo.LatLngs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lat = c.Single(),
                        Lng = c.Single(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSheets", "SiteLocation_Id", "dbo.LatLngs");
            DropForeignKey("dbo.TimeSheets", "LogOnLocation_Id", "dbo.LatLngs");
            DropForeignKey("dbo.TimeSheets", "LogOffLocation_Id", "dbo.LatLngs");
            DropForeignKey("dbo.TimeSheets", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.TimeSheets", new[] { "SiteLocation_Id" });
            DropIndex("dbo.TimeSheets", new[] { "LogOnLocation_Id" });
            DropIndex("dbo.TimeSheets", new[] { "LogOffLocation_Id" });
            DropIndex("dbo.TimeSheets", new[] { "ApplicationUserId" });
            DropTable("dbo.LatLngs");
            DropTable("dbo.TimeSheets");
        }
    }
}
