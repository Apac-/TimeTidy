using NUnit.Framework;
using System.Data.Entity.Migrations;
using TimeTidy.Persistence;
using TimeTidy.Models;
using System.Linq;

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
