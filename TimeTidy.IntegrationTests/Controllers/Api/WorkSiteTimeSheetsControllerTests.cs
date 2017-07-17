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
	public class WorkSiteTimeSheetsControllerTests
	{
        private ApplicationDbContext _context;
        private WorkSiteTimeSheetsController _controller;

        [SetUp]
		public void Setup()
		{
            _context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _controller = new WorkSiteTimeSheetsController(new UnitOfWork(_context, userManager));
		}

        #region GetWorkSiteTimeSheets(id)
        [Test, Isolated]
        public void GetWorkSiteTimeSheets_WorkSiteWithGivenIdNotFound_ShouldReturnEmptyList() 
        {
            var notInDb = 312143;

            var result = _controller.GetWorkSiteTimeSheets(notInDb);

            result.Should().BeOfType<OkNegotiatedContentResult<List<WorkSiteTimeSheetDTO>>>()
                .Which.Content.Should().BeNullOrEmpty();
        }

        [Test, Isolated]
        public void GetWorkSiteTimeSheets_WorkSiteIdFound_ShouldReturnListOfWorkSiteTimeSheetDto()
        {
            var user = _context.Users.First();

            var workSite = _context.WorkSites.Add(new WorkSite
            {
                Name = "new site",
                StreetAddress = "address",
                Lat = 2.5f,
                Lng = 2.5f,
            });

            // Save changes to get context supplied Id
            _context.SaveChanges();

            var sheet = _context.TimeSheets.Add(new TimeSheet
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                WorkSiteId = workSite.Id,
                SiteName = workSite.Name,
                SiteLocation = new LatLng(workSite.Lat, workSite.Lng),
                SiteAddress = workSite.StreetAddress,
                LogOnTime = DateTime.UtcNow,
                LogOnLocation = new LatLng(1.0f, 1.0f),
            });

            _context.SaveChanges();

            var expected = new List<WorkSiteTimeSheetDTO>();
            expected.Add(new WorkSiteTimeSheetDTO(sheet));
        
            // Act
            var result = _controller.GetWorkSiteTimeSheets(workSite.Id);

            // Assert
            result.Should().BeOfType<OkNegotiatedContentResult<List<WorkSiteTimeSheetDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expected);
        }
        #endregion
    }
}
