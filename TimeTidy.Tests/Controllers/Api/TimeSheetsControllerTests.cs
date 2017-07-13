using System;
using NUnit.Framework;
using Moq;
using TimeTidy.Persistence;
using FluentAssertions;
using TimeTidy.Controllers.Api;
using TimeTidy.Persistence.Repositories;
using System.Web.Http.Results;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;
using TimeTidy.Extensions;

namespace TimeTidy.Tests.Controllers.Api
{
    [TestFixture]
    public class TimeSheetsControllerTests
    {
        private TimeSheetsController _controller;
        private Mock<ITimeSheetRepository> _mockTimeSheetRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockTimeSheetRepo = new Mock<ITimeSheetRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.TimeSheets).Returns(_mockTimeSheetRepo.Object);

            _userId = "1";

            _controller = new TimeSheetsController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain.com");
        }

        #region GetTimeSheets
        [Test]
        public void GetTimeSheets_VaildRequestWithIdGiven_ShouldReturnOkWithLogOnTimeDTO()
        {
            var sheet = new TimeSheet() { LogOnTime = new DateTime(1), Id = 1 };
            var expectedReturnDto = new LogOnTimeDTO() { DateTime = new DateTime(1), TimeSheetId = 1 };

            _mockTimeSheetRepo.Setup(r => r.GetCurrentLoggedInSheetForUser(_userId, 1)).Returns(sheet);

            var result = _controller.GetTimeSheets(1);
            var contentResult = result as OkNegotiatedContentResult<LogOnTimeDTO>;

            result.Should().BeOfType<OkNegotiatedContentResult<LogOnTimeDTO>>()
                .Which.Content.ShouldBeEquivalentTo(expectedReturnDto);
        }

        [Test]
        public void GetTimeSheets_VaildRequestWithNoMatchingSiteId_ShouldReturnOkWithNullVaulesInDTO()
        {
            var expectedReturnDto = new LogOnTimeDTO() { DateTime = null, TimeSheetId = null };

            _mockTimeSheetRepo.Setup(r => r.GetCurrentLoggedInSheetForUser(_userId, 1)).Returns(null as TimeSheet);

            var result = _controller.GetTimeSheets(1);
            var contentResult = result as OkNegotiatedContentResult<LogOnTimeDTO>;

            result.Should().BeOfType<OkNegotiatedContentResult<LogOnTimeDTO>>()
                .Which.Content.ShouldBeEquivalentTo(expectedReturnDto);
        }

        #endregion

        #region CreateTimeSheet
        [Test]
        public void CreateTimeSheet_VaildRequest_ShouldReturnOkResult()
        {
            var dto = new TimeSheetLogonDTO
            {
                WorkSiteId = 1,
                SiteName = "place",
                SiteLat = 1.0f,
                SiteLng = 1.0f,
                SiteAddress = "1 place street",
                UserLat = 1.0f,
                UserLng = 1.0f
            };

            var result = _controller.CreateTimeSheet(dto);

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public void CreateTimeSheet_LogonDtoDoesntContainWorkSiteID_ShouldReturnBadRequestWithMessage()
        {
            var dto = new TimeSheetLogonDTO
            {
                SiteName = "place",
                SiteLat = 1.0f,
                SiteLng = 1.0f,
                SiteAddress = "1 place street",
                UserLat = 1.0f,
                UserLng = 1.0f
            };

            var result = _controller.CreateTimeSheet(dto);

            result.Should().BeOfType<BadRequestErrorMessageResult>("No Worksite ID given.");
        }

        [Test]
        public void CreateTimeSheet_LogonDTODoesntContainSiteName_ShouldReturnBadRequestWithMessage()
        {
            var dto = new TimeSheetLogonDTO
            {
                WorkSiteId = 1,
                SiteLat = 1.0f,
                SiteLng = 1.0f,
                SiteAddress = "1 place street",
                UserLat = 1.0f,
                UserLng = 1.0f
            };

            var result = _controller.CreateTimeSheet(dto);

            result.Should().BeOfType<BadRequestErrorMessageResult>("No site name given.");
        }

        [Test]
        public void CreateTimeSheet_LogonDTODoesntContainSiteLatLng_ShouldReturnBadRequestWithMessage()
        {
            var dto = new TimeSheetLogonDTO
            {
                WorkSiteId = 1,
                SiteName = "place",
                SiteAddress = "1 place street",
                UserLat = 1.0f,
                UserLng = 1.0f
            };

            var result = _controller.CreateTimeSheet(dto);

            result.Should().BeOfType<BadRequestErrorMessageResult>("No site location given.");
        }

        [Test]
        public void CreateTimeSheet_LogonDTODoesntContainUserLatLng_ShouldReturnBadRequestWithMessage()
        {
            var dto = new TimeSheetLogonDTO
            {
                WorkSiteId = 1,
                SiteName = "place",
                SiteLat = 1.0f,
                SiteLng = 1.0f,
                SiteAddress = "1 place street",
            };

            var result = _controller.CreateTimeSheet(dto);

            result.Should().BeOfType<BadRequestErrorMessageResult>("No user location given.");

        }
        #endregion 

        #region UpdateTimeSheet
        [Test]
        public void UpdateTimeSheet_NoSheetWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.UpdateTimeSheet(80, new TimeSheetLogoffDTO());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void UpdateTimeSheet_SheetGivenDoesntMatchCurrentUserID_ShouldReturnBadRequest()
        {
            var sheet = new TimeSheet { ApplicationUserId = _userId + "-" };

            _mockTimeSheetRepo.Setup(r => r.GetTimeSheet(1)).Returns(sheet);

            var result = _controller.UpdateTimeSheet(1, new TimeSheetLogoffDTO());

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateTimeSheet_DTODoesntHaveLat_ShouldReturnBadRequest()
        {
            var sheet = new TimeSheet { ApplicationUserId = _userId};
            var dto = new TimeSheetLogoffDTO { UserLng = 1.0f };

            _mockTimeSheetRepo.Setup(r => r.GetTimeSheet(1)).Returns(sheet);

            var result = _controller.UpdateTimeSheet(1, dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateTimeSheet_DTODoesntHaveLng_ShouldReturnBadRequest()
        {
            var sheet = new TimeSheet { ApplicationUserId = _userId};
            var dto = new TimeSheetLogoffDTO { UserLat = 1.0f };

            _mockTimeSheetRepo.Setup(r => r.GetTimeSheet(1)).Returns(sheet);

            var result = _controller.UpdateTimeSheet(1, dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateTimeSheet_ValidRequest_ShouldReturnOkResult()
        {
            var sheet = new TimeSheet { ApplicationUserId = _userId};
            var dto = new TimeSheetLogoffDTO { UserLat = 1.0f, UserLng = 1.0f };

            _mockTimeSheetRepo.Setup(r => r.GetTimeSheet(1)).Returns(sheet);

            var result = _controller.UpdateTimeSheet(1, dto);

            result.Should().BeOfType<OkResult>();
        }
        #endregion 
    }
}
