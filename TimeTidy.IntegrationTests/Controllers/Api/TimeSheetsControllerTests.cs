using FluentAssertions;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Http.Results;
using TimeTidy.Controllers.Api;
using TimeTidy.Extensions;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;
using TimeTidy.Persistence;

namespace TimeTidy.IntegrationTests.Controllers.Api
{
	[TestFixture]
	public class TimeSheetsControllerTests
	{
        private ApplicationDbContext _context;
        private TimeSheetsController _controller;

        [SetUp]
		public void Setup()
		{
            _context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _controller = new TimeSheetsController(new UnitOfWork(_context, userManager));
		}

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region GetTimeSheets(id)
        [Test]
        public void GetTimeSheets_SuppliedWorkSiteIdNotFoundInDb_ShouldReturnNullValuesInDTO()
        {
            var user = _context.Users.First();

            _controller.MockCurrentUser(user.Id, user.UserName);

            var expected = new LogOnTimeDTO { DateTime = null, TimeSheetId = null };

            var result = _controller.GetTimeSheets(999);

            result.Should().BeOfType<OkNegotiatedContentResult<LogOnTimeDTO>>()
                .Which.Content.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void GetTimeSheets_CurrentUserDoesNotHaveAnySites_ShouldReturnNullValuesInDTO()
        {
            _controller.MockCurrentUser("FakeUserId", "UserName");

            var expected = new LogOnTimeDTO { DateTime = null, TimeSheetId = null };

            var result = _controller.GetTimeSheets(9999);

            result.Should().BeOfType<OkNegotiatedContentResult<LogOnTimeDTO>>()
                .Which.Content.ShouldBeEquivalentTo(expected);
        }

        [Test, Isolated]
        public void GetTimeSheets_TimeSheetFoundForUserAndSiteID_ShouldReturnFilledDTO()
        {
            // Arrange
            var user = _context.Users.First();

            _controller.MockCurrentUser(user.Id, user.UserName);

            var workSite = new WorkSite
            {
                Id = 5050, // Random
                Name = "site 5050",
                StreetAddress = "address",
                Lat = 5.0f,
                Lng = 5.0f,
            };

            var sheet = new TimeSheet
            {
                ApplicationUser = user,
                WorkSiteId = workSite.Id,
                SiteName = workSite.Name,
                LogOnTime = DateTime.Now,
            };

            var sheetAdded = _context.TimeSheets.Add(sheet);
            _context.SaveChanges();

            // Act
            var result = _controller.GetTimeSheets(workSite.Id);

            // Assert

            // Get sheet from db to check vs return dto
            var sheetInDb = _context.TimeSheets.Single(t => t.Id == sheetAdded.Id);

            result.Should().BeOfType<OkNegotiatedContentResult<LogOnTimeDTO>>();

            result.As<OkNegotiatedContentResult<LogOnTimeDTO>>()
                .Content.DateTime.ShouldBeEquivalentTo(sheetInDb.LogOnTime);
            result.As<OkNegotiatedContentResult<LogOnTimeDTO>>()
                .Content.TimeSheetId.ShouldBeEquivalentTo(sheetInDb.Id);
        }
        #endregion

        #region CreateTimeSheet(TimeSheetLogonDTO)
        [Test]
        public void CreateTimeSheet_UserNotFound_ShouldReturnNotFoundResult()
        {
            var result = _controller.CreateTimeSheet(new TimeSheetLogonDTO());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void CreateTimeSheet_AddNewTimeSheetToDb_ShouldAddSheetToDb()
        {
            // Arrange
            var user = _context.Users.First();

            _controller.MockCurrentUser(user.Id, user.UserName);

            var dto = new TimeSheetLogonDTO
            {
                WorkSiteId = 12931, // Random
                SiteName = "dto sitename",
                SiteLat = 9.0f,
                SiteLng = 9.0f,
                SiteAddress = "dto address",
                UserLat = 10.0f,
                UserLng = 10.0f,
            };

            var expectedTimeSheet = new TimeSheet
            {
                ApplicationUserId = user.Id,
                WorkSiteId = dto.WorkSiteId,
                SiteName = dto.SiteName,
                SiteLocation = new LatLng(dto.SiteLat, dto.SiteLng),
                SiteAddress = dto.SiteAddress,
                LogOnTime = DateTime.UtcNow, // Not checking this in the db as the times will differ
                LogOnLocation = new LatLng(dto.UserLat, dto.UserLng),
            };

            // Act
            var result = _controller.CreateTimeSheet(dto);

            // Assert

            result.Should().BeOfType<OkResult>();

            // Find new sheet
            var sheetInDb = _context.TimeSheets
                .Single(t => t.WorkSiteId == expectedTimeSheet.WorkSiteId);

            sheetInDb.ApplicationUserId.ShouldBeEquivalentTo(expectedTimeSheet.ApplicationUserId);

            sheetInDb.SiteName.ShouldBeEquivalentTo(expectedTimeSheet.SiteName);

            sheetInDb.SiteLocation.Lat.ShouldBeEquivalentTo(expectedTimeSheet.SiteLocation.Lat);
            sheetInDb.SiteLocation.Lng.ShouldBeEquivalentTo(expectedTimeSheet.SiteLocation.Lng);

            sheetInDb.SiteAddress.ShouldBeEquivalentTo(expectedTimeSheet.SiteAddress);

            sheetInDb.LogOnLocation.Lat.ShouldBeEquivalentTo(expectedTimeSheet.LogOnLocation.Lat);
            sheetInDb.LogOnLocation.Lng.ShouldBeEquivalentTo(expectedTimeSheet.LogOnLocation.Lng);
        }
        #endregion

        #region UpdateTimeSheet(id, TimeSheetLogoffDTO)
        [Test, Isolated]
        public void UpdateTimeSheet_TimeSheetAndUserFoundThenUpdated_ShouldUpdateTimeSheetInDbWithDtoAndReturnOKResult()
        {
            // Arrange

            var user = _context.Users.First();

            var dto = new TimeSheetLogoffDTO { UserLat = 5.0f, UserLng = 5.0f }; 

            var sheet = new TimeSheet
            {
                ApplicationUserId = user.Id,
                WorkSiteId = 5252, // Random
                SiteName = "Site name 5252",
                LogOnTime = DateTime.Now,
            };

            // Add new sheet to DB
            var sheetInDb = _context.TimeSheets.Add(sheet);
            _context.SaveChanges();

            _controller.MockCurrentUser(user.Id, user.UserName);

            // Act
            var result = _controller.UpdateTimeSheet(sheetInDb.Id, dto);

            result.Should().BeOfType<OkResult>();

            // Get updated sheet from db
            var updatedSheet = _context.TimeSheets.Single(t => t.Id == sheetInDb.Id);
            updatedSheet.LogOffTime.Should().NotBeNull();
            updatedSheet.LogOffLocation.Lat.ShouldBeEquivalentTo(dto.UserLat);
            updatedSheet.LogOffLocation.Lng.ShouldBeEquivalentTo(dto.UserLng);
        }

        [Test, Isolated]
        public void UpdateTimeSheet_SheetWithGivenIdNotFoundInDb_ShouldNotUpdateAnythingAndReturnNotFound()
        {
            var result = _controller.UpdateTimeSheet(0109238, new TimeSheetLogoffDTO());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void UpdateTimeSheet_CurrentUserDoesNotMatchSheetUserId_ShouldNotUpdateAnythingAndReturnBadRequest()
        {
            var users = _context.Users.ToArray();
            var loggedInUser = users[0];
            var otherUser = users[1];

            var sheet = new TimeSheet
            {
                // User ID here is NOT the same as the Currently logged in user.
                ApplicationUserId = otherUser.Id,
                WorkSiteId = 5252, // Random
                SiteName = "Site name 5252",
                LogOnTime = DateTime.Now,
            };

            var sheetInDb = _context.TimeSheets.Add(sheet);
            _context.SaveChanges();
            
            // Set currently logged in user to UserOne. Sheet is UserTwo
            _controller.MockCurrentUser(loggedInUser.Id, loggedInUser.UserName);

            var dto = new TimeSheetLogoffDTO { UserLat = 5.0f, UserLng = 5.0f }; 

            // Act
            var result = _controller.UpdateTimeSheet(sheetInDb.Id, dto);

            // Assert
            result.Should().BeOfType<BadRequestResult>();

            // Make sure sheet isn't updated
            var updatedSheet = _context.TimeSheets.Single(t => t.Id == sheetInDb.Id);
            updatedSheet.LogOffLocation.Should().BeNull();
            updatedSheet.LogOffTime.Should().BeNull();
        }
        #endregion
    }
}
