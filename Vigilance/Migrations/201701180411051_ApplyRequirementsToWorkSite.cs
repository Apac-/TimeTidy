namespace TimeTidy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyRequirementsToWorkSite : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkSites", "Name", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkSites", "Name", c => c.String());
        }
    }
}
