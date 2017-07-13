using NUnit.Framework;
using TimeTidy.Controllers;
using TimeTidy.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;
using TimeTidy.Extensions;
using System.Linq;
using TimeTidy.ViewModels;
using FluentAssertions;
using System.Web.Mvc;

namespace TimeTidy.IntegrationTests.Controllers
{
    [TestFixture]
    public class TimeSheetsControllerTests
    {
        private TimeSheetsController _controller;
        private ApplicationDbContext _context;

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

        #region List(id)
        [Test]
        public void List_UserIdInDb_ShouldReturnViewResultWithUserIdAndNameInViewModel()
        {
            var user = _context.Users.First();

            var expected = new TimeSheetsListViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
            };

            var result = _controller.List(user.Id);

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void List_UserNotFoundInDb_ShouldReturnNotFoundResult()
        {
            var result = _controller.List("Not real Id");

            result.Should().BeOfType<HttpNotFoundResult>();
        }
        #endregion

        #region WorkSite(id)
        [Test]
        public void WorkSite_WorkSiteWithGivenIdNotFoundInDb_ShouldReturnNotFoundResult()
        {
            var result = _controller.Worksite(999);

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test, Isolated]
        public void WorkSite_SiteFoundInDb_ShouldReturnViewResultWithSiteFromDb()
        {
            var worksite = new WorkSite
            {
                Id = 1,
                Name = "Site one",
                StreetAddress = "Address",
                Lat = 1.0f,
                Lng = 1.0f,
            };
            _context.WorkSites.Add(worksite);
            _context.SaveChanges();

            var result = _controller.Worksite(worksite.Id);

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(worksite);
        }
        #endregion
    }
}
