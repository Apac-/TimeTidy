using System;
using NUnit.Framework;
using System.Data.Entity.Migrations;

namespace TimeTidy.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetUp
    {
        [SetUp]
        public void SetUp()
        {
            MigrateDbToLatestVersion();
        }

        private static void MigrateDbToLatestVersion()
        {
            var config = new TimeTidy.Migrations.Configuration();
            var migrator = new DbMigrator(config);
        }
    }
}
