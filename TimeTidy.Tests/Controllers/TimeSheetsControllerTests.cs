using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using TimeTidy.Controllers;
using TimeTidy.Models;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using TimeTidy.ViewModels;
using TimeTidy.Extensions;

namespace TimeTidy.Tests.Controllers
{
    [TestFixture]
    public class TimeSheetsControllerTests
    {
        private TimeSheetsController _controller;
        private Mock<IApplicationUserRepository> _mockAppUserRepo;
        private Mock<IWorkSiteRepository> _mockWorksiteRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockAppUserRepo = new Mock<IApplicationUserRepository>();
            _mockWorksiteRepo = new Mock<IWorkSiteRepository>();
            
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Users).Returns(_mockAppUserRepo.Object);
            mockUoW.SetupGet(u => u.WorkSites).Returns(_mockWorksiteRepo.Object);

            _userId = "1";

            _controller = new TimeSheetsController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain");
        }

        #region List(id)
        [Test]
        public void List_UserNotFound_ShouldReturnNotFoundResult()
        {
            _mockAppUserRepo.Setup(r => r.GetUser(_userId)).Returns(null as ApplicationUser);

            var result = _controller.List(_userId);

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void List_FoundUserWithGivenIdAndReturnedViewModel_ShouldReturnViewResultWithViewModel()
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = "Name",
                Id = _userId
            };

            var vm = new TimeSheetsListViewModel
            {
                UserId = _userId,
                UserName = user.UserName
            };

            _mockAppUserRepo.Setup(r => r.GetUser(_userId)).Returns(user);

            var result = _controller.List(_userId);

            result.Should().BeOfType<ViewResult>()
                .Which.Model.As<TimeSheetsListViewModel>().ShouldBeEquivalentTo(vm);
        }
        #endregion

        #region WorkSite(id)
        [Test]
        public void Worksite_WorksiteNotFoundWithGivenId_ShouldReturnNotFoundResult()
        {
            _mockWorksiteRepo.Setup(r => r.GetWorkSite(1)).Returns(null as WorkSite);

            var result = _controller.Worksite(1);

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Worksite_WorksiteFoundAndReturned_ShouldReturnViewResultWithViewNameAndFoundSite()
        {
            WorkSite site = new WorkSite
            {
                Id = 1,
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorksiteRepo.Setup(r => r.GetWorkSite(1)).Returns(site);

            var result = _controller.Worksite(1);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("worksite");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(site);

        }
        #endregion
    }
}
