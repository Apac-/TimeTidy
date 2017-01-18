namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkSites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        Radius = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkSites");
        }
    }
}
