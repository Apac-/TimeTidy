using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Mvc;
using TimeTidy.Controllers;
using TimeTidy.Models;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Extensions;

namespace TimeTidy.Tests.Controllers
{
    [TestFixture]
    public class WorkSitesControllerTests
    {
        private WorkSitesController _controller;
        private Mock<IWorkSiteRepository> _mockWorkSiteRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockWorkSiteRepo = new Mock<IWorkSiteRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.WorkSites).Returns(_mockWorkSiteRepo.Object);

            _controller = new WorkSitesController(mockUoW.Object);

            _userId = "1";

            _controller.MockCurrentUser(_userId, "user@domain.com");
        }

        #region Edit(id)
        [Test]
        public void Edit_WorkSiteOfGivenIdNotFound_ShouldReturnHttpNotFoundResult()
        {
            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(null as WorkSite);

            var result = _controller.Edit(1);

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Edit_WorkSiteFoundAndReturned_ShouldReturnViewResultWithViewOfWorkSiteFormAndWorksite()
        {
            WorkSite expectedSite = new WorkSite
            {
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(expectedSite);

            var result = _controller.Edit(1);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("WorkSiteForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSite);
        }
        #endregion

        #region Save(WorkSite)
        [Test]
        public void Save_ModelStateIsNotValid_ShouldReturnViewResultToWorkSiteFormWithGivenWorkSite()
        {
            WorkSite expectedSite = new WorkSite
            {
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(expectedSite);

            _controller.ModelState.AddModelError("error", "error");

            var result = _controller.Save(expectedSite);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("WorkSiteForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSite);
        }

        [Test]
        public void Save_WorkSiteDoesNotContainRequiredFieldName_ShouldReturnViewResultOfWorkSiteFormWithGivenWorkSiteAndModelError()
        {
            WorkSite expectedSite = new WorkSite
            {
                //Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(expectedSite);

            KeyValuePair<string, string> expectedModelError =
                new KeyValuePair<string, string>("Error", "Required Worksite info not supplied.");

            var result = _controller.Save(expectedSite);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("WorkSiteForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSite);
            _controller.ModelState.Should()
                .ContainValue(_controller.ModelState[expectedModelError.Key], expectedModelError.Value);
        }

        [Test]
        public void Save_WorkSiteDoesNotContainRequiredFieldAddress_ShouldReturnViewResultOfWorkSiteFormWithGivenWorkSiteAndModelError()
        {
            WorkSite expectedSite = new WorkSite
            {
                Name = "name",
                //StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(expectedSite);

            KeyValuePair<string, string> expectedModelError =
                new KeyValuePair<string, string>("Error", "Required Worksite info not supplied.");

            var result = _controller.Save(expectedSite);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("WorkSiteForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSite);
            _controller.ModelState.Should()
                .ContainValue(_controller.ModelState[expectedModelError.Key], expectedModelError.Value);
        }

        [Test]
        public void Save_WorkSiteDoesNotContainRequiredFieldLat_ShouldReturnViewResultOfWorkSiteFormWithGivenWorkSiteAndModelError()
        {
            WorkSite expectedSite = new WorkSite
            {
                Name = "name",
                StreetAddress = "address",
                //Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(expectedSite);

            KeyValuePair<string, string> expectedModelError =
                new KeyValuePair<string, string>("Error", "Required Worksite info not supplied.");

            var result = _controller.Save(expectedSite);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("WorkSiteForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSite);
            _controller.ModelState.Should()
                .ContainValue(_controller.ModelState[expectedModelError.Key], expectedModelError.Value);
        }

        [Test]
        public void Save_WorkSiteDoesNotContainRequiredFieldLng_ShouldReturnViewResultOfWorkSiteFormWithGivenWorkSiteAndModelError()
        {
            WorkSite expectedSite = new WorkSite
            {
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                //Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(expectedSite);

            KeyValuePair<string, string> expectedModelError =
                new KeyValuePair<string, string>("Error", "Required Worksite info not supplied.");

            var result = _controller.Save(expectedSite);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("WorkSiteForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSite);
            _controller.ModelState.Should()
                .ContainValue(_controller.ModelState[expectedModelError.Key], expectedModelError.Value);
        }

        [Test]
        public void Save_NewWorkSiteAddedToContext_ShouldReturnRedirectToRouteResultListOfWorkSitesController()
        {
            WorkSite site = new WorkSite
            {
                Id = 0, // Not currently in db yet as it doesn't have Id assigned.
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(site);

            var result = _controller.Save(site);

            result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Action"].ShouldBeEquivalentTo("List");
            result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Controller"].ShouldBeEquivalentTo("WorkSites");
        }

        [Test]
        public void Save_ExistingWorkSiteUpdatedInContext_ShouldReturnRedirectToRouteResultListOfWorkSitesController()
        {
            WorkSite site = new WorkSite
            {
                Id = 1, // Already in db as it has a non 0 value on Id.
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSiteRepo.Setup(r => r.GetWorkSite(1)).Returns(site);

            var result = _controller.Save(site);

            result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Action"].ShouldBeEquivalentTo("List");
            result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Controller"].ShouldBeEquivalentTo("WorkSites");
        }
        #endregion

        #region List()
        [Test]
        public void List_WorkSitesFoundAndReturned_ShouldReturnViewResultWithListOfWorkSites()
        {
            WorkSite site = new WorkSite
            {
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            List<WorkSite> expectedList = new List<WorkSite>();
            expectedList.Add(site);

            _mockWorkSiteRepo.Setup(r => r.GetWorkSites()).Returns(expectedList);

            var result = _controller.List();

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedList);
        }

        [Test]
        public void List_NoWorkSitesFoundAndReturned_ShouldReturnViewResultWithListOfWorkSites()
        {
            _mockWorkSiteRepo.Setup(r => r.GetWorkSites()).Returns(null as List<WorkSite>);

            var result = _controller.List();

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(null as List<WorkSite>);
        }
        #endregion
    }
}
