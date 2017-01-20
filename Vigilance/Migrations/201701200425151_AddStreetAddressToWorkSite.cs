namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStreetAddressToWorkSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkSites", "StreetAddress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkSites", "StreetAddress");
        }
    }
}
