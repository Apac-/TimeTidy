namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'46011ba9-217a-4dac-b903-991e08a96333', N'guest@vigilance.com', 0, N'AKYtfEHDNp/6DjQgCz8lnP8eA2mSzB2lOawsuS0OWU0Yh2QCvyaXzPLtc/VCtjpXnQ==', N'cdcc7cc8-752a-42cb-bd0e-0479a4edfbf6', NULL, 0, 0, NULL, 1, 0, N'guest@vigilance.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'5d7c9b23-ead5-463d-b9c3-7a9260c91161', N'admin@vigilance.com', 0, N'ALD7J76fnP7VB96lLsb2hD05O+GwBP7IbHW+V+AcV+fCRIdDrj/PiqrTXQ3XWc1Wjw==', N'3916dfb8-f771-448e-be21-c6012056b47c', NULL, 0, 0, NULL, 1, 0, N'admin@vigilance.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'ca3e827a-956e-4ace-b5c2-1514ee958527', N'CanManageWorkSites')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5d7c9b23-ead5-463d-b9c3-7a9260c91161', N'ca3e827a-956e-4ace-b5c2-1514ee958527')
");
        }
        
        public override void Down()
        {
        }
    }
}
