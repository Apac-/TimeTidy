namespace Vigilance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3e0580f6-dfbc-4a62-9020-15c6dad3d970', N'admin@timetidy.com', 0, N'AEnsyxWvDBu8/SaGywLpXUQLB1nGsQWNFWxXAw6zejq0Sytx29HUTGxE/oRhDzBv5w==', N'69d2dcb7-0645-4cdc-b338-e684a5af0136', N'0', 0, 0, NULL, 1, 0, N'admin@timetidy.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'02a9feef-f801-4a71-85a6-670a30e5ffb8', N'CanManageUsers')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'ca3e827a-956e-4ace-b5c2-1514ee958527', N'CanManageWorkSites')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3e0580f6-dfbc-4a62-9020-15c6dad3d970', N'02a9feef-f801-4a71-85a6-670a30e5ffb8')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3e0580f6-dfbc-4a62-9020-15c6dad3d970', N'ca3e827a-956e-4ace-b5c2-1514ee958527')
");
        }
        
        public override void Down()
        {
        }
    }
}
