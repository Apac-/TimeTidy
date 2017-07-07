using System;
using NUnit.Framework;
using TimeTidy.Persistence.Repositories;
using Moq;
using TimeTidy.Persistence;
using TimeTidy.Controllers.Api;
using TimeTidy.Tests.Extensions;
using TimeTidy.Models;
using System.Collections.Generic;
using TimeTidy.Models.DTOs;
using FluentAssertions;
using System.Web.Http.Results;

namespace TimeTidy.Tests.Controllers.Api
{
	[TestFixture]
	public class UserTimeSheetsCoontrollerTests
	{
        private UserTimeSheetsController _controller;
        private Mock<ITimeSheetRepository> _mockTimeSheetRepo;
        private string _userId;

        [SetUp]
		public void Setup()
		{
            _mockTimeSheetRepo = new Mock<ITimeSheetRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.TimeSheets).Returns(_mockTimeSheetRepo.Object);

            _userId = "1";

            _controller = new UserTimeSheetsController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain.com");
		}

        #region GetUserTimeSheets(id)
        [Test]
        public void GetUserTimeSheets_ValidRequest_ShouldReturnOkResultWithListOfTimeSheetDTO()
        {
            var sheetOne = new TimeSheet()
            {
                Id = 1,
                SiteName = "s 1",
                SiteLocation = new LatLng(1.0f, 1.0f),
                SiteAddress = "address",
                LogOnTime = new DateTime(1),
                LogOnLocation = new LatLng(1.0f, 1.0f),
                LogOffTime = new DateTime(2),
                LogOffLocation = new LatLng(1.0f, 1.0f),
            };

            List<TimeSheet> sheets = new List<TimeSheet>();
            sheets.Add(sheetOne);

            _mockTimeSheetRepo.Setup(r => r.GetTimeSheetsByUser(_userId)).Returns(sheets);

            List<TimeSheetDTO> expectedResult = new List<TimeSheetDTO>();

            foreach (var sheet in sheets)
            {
                expectedResult.Add(new TimeSheetDTO(sheet));
            }

            var result = _controller.GetUserTimeSheets(_userId);

            result.Should().BeOfType<OkNegotiatedContentResult<List<TimeSheetDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expectedResult);
        }
        #endregion

        #region DeleteTimeSheet(id)
        [Test]
        public void DeleteTimeSheet_SheetWithGivenIdIsNotFound_ShouldReturnNotFoundResult()
        {
            _mockTimeSheetRepo.Setup(r => r.GetTimeSheet(1)).Returns(null as TimeSheet);

            var result = _controller.DeleteTimeSheet(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void DeleteTimeSheet_ValidRequest_ShouldReturnOkResult()
        {
            _mockTimeSheetRepo.Setup(r => r.GetTimeSheet(1)).Returns(new TimeSheet());

            var result = _controller.DeleteTimeSheet(1);

            result.Should().BeOfType<OkResult>();
        }
        #endregion
    }
}
