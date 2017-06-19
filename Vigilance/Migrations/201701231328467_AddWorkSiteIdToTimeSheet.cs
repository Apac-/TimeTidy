namespace TimeTidy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkSiteIdToTimeSheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheets", "WorkSiteId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheets", "WorkSiteId");
        }
    }
}
