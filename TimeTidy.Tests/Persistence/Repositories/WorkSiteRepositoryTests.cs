using Moq;
using NUnit.Framework;
using System;
using System.Data.Entity;
using TimeTidy.Models;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Tests.Extensions;

namespace TimeTidy.Tests.Persistence.Repositories
{
    [TestFixture]
    public class WorkSiteRepositoryTests
    {
        private Mock<DbSet<WorkSite>> _mockWorkSites;
        private WorkSiteRepository _repository;
        private WorkSite _siteOne;

        [SetUp]
        public void Setup()
        {
            _siteOne = new WorkSite()
            {
                Id = 1,
                Name = "Site one",
                StreetAddress = "Address one",
                Lat = 1.0f,
                Lng = 1.0f,
            };

            _mockWorkSites = new Mock<DbSet<WorkSite>>();
            _mockWorkSites.SetSource(new[] { _siteOne });

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.WorkSites).Returns(_mockWorkSites.Object);

            _repository = new WorkSiteRepository(mockContext.Object);
        }
    }
}
