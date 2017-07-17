using FluentAssertions;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using TimeTidy.Controllers.Api;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;
using TimeTidy.Persistence;

namespace TimeTidy.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class UserTimeSheetsControllerTests
    {
        private ApplicationDbContext _context;
        private UserTimeSheetsController _controller;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _controller = new UserTimeSheetsController(new UnitOfWork(_context, userManager));
        }

        #region GetUserTimeSheets(id)
        [Test, Isolated]
        public void GetUserTimeSheets_ValidRequest_ShouldReturnListOfTimeSheetDtos()
        {
            var user = _context.Users.First();

            var sheetOne = new TimeSheet
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                WorkSiteId = 1,
                SiteName = "site one",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = DateTime.Now,
                LogOnLocation = new LatLng(1.0f, 1.0f),
            };

            var sheetTwo = new TimeSheet
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                WorkSiteId = 2,
                SiteName = "site two",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = DateTime.Now,
                LogOnLocation = new LatLng(1.0f, 1.0f),
            };

            var sheetOneInDb = _context.TimeSheets.Add(sheetOne);
            var sheetTwoInDb = _context.TimeSheets.Add(sheetTwo);

            _context.SaveChanges();

            var expected = new List<TimeSheetDTO>();
            expected.Add(new TimeSheetDTO(sheetOneInDb));
            expected.Add(new TimeSheetDTO(sheetTwoInDb));

            // Act
            var result = _controller.GetUserTimeSheets(user.Id);

            // Assert
            result.Should().BeOfType<OkNegotiatedContentResult<List<TimeSheetDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expected);
        }

        [Test, Isolated]
        public void GetUserTimeSheets_NoSheetsFoundForUser_ShouldReturnEmptyListOfTimeSheetDtos()
        {
            var expected = new List<TimeSheetDTO>();

            var result = _controller.GetUserTimeSheets("NotRealUserID");

            result.Should().BeOfType<OkNegotiatedContentResult<List<TimeSheetDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expected);
        }
        #endregion

        #region DeleteTimeSheet(id)
        [Test, Isolated]
        public void DeleteTimeSheet_SheetNotFound_ShouldReturnNotFoundResult()
        {
            var notRealId = 12314;

            var result = _controller.DeleteTimeSheet(notRealId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void DeleteTimeSheet_SheetFoundAndRemovedFromDb_ShouldDeleteSheetFromDbAndReturnOk()
        {
            var user = _context.Users.First();

            var sheet = new TimeSheet
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                WorkSiteId = 1,
                SiteName = "site one",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = DateTime.Now,
                LogOnLocation = new LatLng(1.0f, 1.0f),
            };

            var sheetInDb = _context.TimeSheets.Add(sheet);

            _context.SaveChanges();

            // Act
            var result = _controller.DeleteTimeSheet(sheetInDb.Id);

            // Assert
            result.Should().BeOfType<OkResult>();

            var sheetFound = _context.TimeSheets.SingleOrDefault(c => c.Id == sheetInDb.Id);
            sheetFound.Should().BeNull();
        }
        #endregion
    }
}
