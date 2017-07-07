using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using FluentAssertions;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Persistence;
using TimeTidy.Controllers.Api;
using TimeTidy.Tests.Extensions;
using System.Collections.Generic;
using TimeTidy.Models;
using System.Web.Http.Results;

namespace TimeTidy.Tests.Controllers.Api
{
    [TestFixture]
    public class WorkSitesControllerTests
    {
        private WorkSitesController _controller;
        private Mock<IWorkSiteRepository> _mockWorkSitesRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockWorkSitesRepo = new Mock<IWorkSiteRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.WorkSites).Returns(_mockWorkSitesRepo.Object);

            _userId = "1";

            _controller = new WorkSitesController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain.com");
        }

        #region GetWorkSites
        [Test]
        public void GetWorkSites_ValidRequest_ShouldReturnOkResultWithIEnumWorkSites()
        {
            WorkSite siteOne = new WorkSite()
            {
                Id = 1,
                Name = "site name",
                StreetAddress = "1 site street",
                Lat = 1.0f,
                Lng = 1.0f
            };

            List<WorkSite> expectedResult = new List<WorkSite>();
            expectedResult.Add(siteOne);

            _mockWorkSitesRepo.Setup(r => r.GetWorkSites()).Returns(expectedResult);

            var result = _controller.GetWorkSites();

            result.Should().BeOfType<OkNegotiatedContentResult<IEnumerable<WorkSite>>>()
                .Which.Content.ShouldBeEquivalentTo(expectedResult);
        }
        #endregion

        #region GetWorkSite(id)
        [Test]
        public void GetWorkSite_GivenIdNotFound_ShouldReturnNotFoundResult()
        {
            _mockWorkSitesRepo.Setup(r => r.GetWorkSite(1)).Returns(null as WorkSite);

            var result = _controller.GetWorkSite(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void GetWorkSite_ValidRequest_ShouldReturnOkWithFoundWorkSite()
        {
            WorkSite siteOne = new WorkSite()
            {
                Id = 1,
                Name = "site name",
                StreetAddress = "1 site street",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSitesRepo.Setup(r => r.GetWorkSite(1)).Returns(siteOne);

            var result = _controller.GetWorkSite(1);

            result.Should().BeOfType<OkNegotiatedContentResult<WorkSite>>()
                .Which.Content.ShouldBeEquivalentTo(siteOne);
        }
        #endregion
    }
}
