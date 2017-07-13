using Moq;
using NUnit.Framework;
using TimeTidy.Controllers.Api;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Persistence;
using TimeTidy.Models.DTOs;
using TimeTidy.Extensions;
using System.Collections.Generic;
using TimeTidy.Models;
using FluentAssertions;
using System.Web.Http.Results;

namespace TimeTidy.Tests.Controllers.Api
{
    [TestFixture]
    public class WorkSiteTimeSheetsControllerTests
    {
        private WorkSiteTimeSheetsController _controller;
        private Mock<ITimeSheetRepository> _mockTimeSheetRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockTimeSheetRepo = new Mock<ITimeSheetRepository>();

            _userId = "1";

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.TimeSheets).Returns(_mockTimeSheetRepo.Object);

            _controller = new WorkSiteTimeSheetsController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain.com");
        }

        #region GetWorkSiteTimeSheets(id)
        [Test]
        public void GetWorkSiteTimeSheets_ValidRequest_ShouldReturnOkResultWithDto()
        {
            ApplicationUser user = new ApplicationUser
            {
                FirstName = "first",
                LastName = "Last"
            };

            TimeSheet sheetOne = new TimeSheet
            {
                ApplicationUser = user,
                Id = 1,
                ApplicationUserId = _userId,
                WorkSiteId = 1,
                SiteName = "s1"
            };

            TimeSheet sheetTwo = new TimeSheet
            {
                ApplicationUser = user,
                Id = 2,
                ApplicationUserId = _userId,
                WorkSiteId = 1,
                SiteName = "s1"
            };

            List<TimeSheet> sheets = new List<TimeSheet>();
            sheets.Add(sheetOne);
            sheets.Add(sheetTwo);

            List<WorkSiteTimeSheetDTO> expectedDto = new List<WorkSiteTimeSheetDTO>();
            foreach (var sheet in sheets)
            {
                expectedDto.Add(new WorkSiteTimeSheetDTO(sheet));
            }

            _mockTimeSheetRepo.Setup(r => r.GetTimeSheetsByWorkSite(1)).Returns(sheets);

            var result = _controller.GetWorkSiteTimeSheets(1);

            result.Should().BeOfType<OkNegotiatedContentResult<List<WorkSiteTimeSheetDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expectedDto);
        }
        #endregion
    }
}
