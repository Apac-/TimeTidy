using NUnit.Framework;
using System.Data.Entity.Migrations;
using TimeTidy.Persistence;
using TimeTidy.Models;
using System.Linq;
using System;

namespace TimeTidy.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetUp
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            MigrateDbToLatestVersion();

            Seed();
        }

        private void Seed()
        {
            var context = new ApplicationDbContext();

            SeedUsers(context);

            SeedWorkSites(context);
        }

        private void SeedWorkSites(ApplicationDbContext context)
        {
            if (context.WorkSites.Count() > 1)
                return;

            context.WorkSites.Add(new WorkSite { Name = "Site one", StreetAddress = "1 street", Lat = 1.0f, Lng = 1.0f });
            context.WorkSites.Add(new WorkSite { Name = "Site Two", StreetAddress = "2 street", Lat = 2.0f, Lng = 2.0f });

            context.SaveChanges();
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            if (context.Users.Count() > 1)
                return;

            context.Users.Add(new ApplicationUser { UserName = "user1", FirstName = "User", LastName = "One", Email = "user1@domain.com", PasswordHash = "-" });
            context.Users.Add(new ApplicationUser { UserName = "user2", FirstName = "User", LastName = "Two", Email = "user2@domain.com", PasswordHash = "-" });

            context.SaveChanges();
        }

        private static void MigrateDbToLatestVersion()
        {
            var config = new TimeTidy.Migrations.Configuration();
            var migrator = new DbMigrator(config);
            migrator.Update();
        }
    }
}
