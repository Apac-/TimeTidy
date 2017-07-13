using FluentAssertions;
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

        #region GetWorkSite(id)
        [Test]
        public void GetWorkSite_SiteIdNotFound_ShouldReturnNull()
        {
            var result = _repository.GetWorkSite(_siteOne.Id + 999);

            result.Should().BeNull();
        }

        [Test]
        public void GetWorkSite_SiteFound_ShouldReturnFoundSite()
        {
            var result = _repository.GetWorkSite(_siteOne.Id);

            result.ShouldBeEquivalentTo(_siteOne);
        }
        #endregion

        #region GetWorkSites()
        [Test]
        public void GetWorkSites_ValidRequest_ShouldReturnAllSites()
        {
            var result = _repository.GetWorkSites();

            result.ShouldBeEquivalentTo(new[] { _siteOne }); 
        }
        #endregion
    }
}
