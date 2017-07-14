using FluentAssertions;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using TimeTidy.Controllers;
using TimeTidy.Models;
using TimeTidy.Persistence;

namespace TimeTidy.IntegrationTests.Controllers
{
    [TestFixture]
    public class WorkSitesControllerTests
    {
        private ApplicationDbContext _context;
        private WorkSitesController _controller;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _controller = new WorkSitesController(new UnitOfWork(_context, userManager));
        }

        #region Edit(id)
        [Test]
        public void Edit_WorkSiteNotFoundInDb_ShouldReturnHttpNotFound()
        {
            var result = _controller.Edit(9999);

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Edit_WorkSiteFoundInDb_ShouldReturnViewResultWithFoundWorkSite()
        {
            var worksite = _context.WorkSites.First();

            var result = _controller.Edit(worksite.Id);

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(worksite); 
        }
        #endregion

        #region Save(worksite)
        [Test, Isolated]
        public void Save_NewWorkSiteAddedToDb_ShouldAddNewWorkSiteToDb()
        {
            var worksite = new WorkSite
            {
                Id = 0, // 0 means it isn't currently in db
                Name = "New site has been added",
                StreetAddress = "New address",
                Lat = 333.0f,
                Lng = 222.0f,
            };

            var result = _controller.Save(worksite);

            var resultSite = _context.WorkSites.SingleOrDefault(w => w.Name == worksite.Name);
            resultSite.Should().NotBeNull();

            resultSite.Name.ShouldBeEquivalentTo(worksite.Name);
            resultSite.StreetAddress.ShouldBeEquivalentTo(worksite.StreetAddress);
            resultSite.Lat.ShouldBeEquivalentTo(worksite.Lat);
            resultSite.Lng.ShouldBeEquivalentTo(worksite.Lng);

            result.Should().BeOfType<RedirectToRouteResult>();
        }

        [Test, Isolated]
        public void Save_WorkSiteUpdatedInDb_ShouldUpdateDbWorkSiteWithPassedInWorkSite()
        {
            var siteInDb = _context.WorkSites.First();

            var expectedSite = new WorkSite
            {
                Id = siteInDb.Id,
                Name = siteInDb.Name + "-",
                StreetAddress= siteInDb.StreetAddress + "-",
                Lat = siteInDb.Lat + 1.0f,
                Lng = siteInDb.Lng + 1.0f,
            };

            var result = _controller.Save(expectedSite);

            var updatedSiteInDb = _context.WorkSites.Single(w => w.Id == siteInDb.Id);
            updatedSiteInDb.ShouldBeEquivalentTo(expectedSite);

            result.Should().BeOfType<RedirectToRouteResult>();
        }
        #endregion

        #region List()
        [Test]
        public void List_ValidCall_ShouldReturnViewResultWithAllWorkSitesInDb()
        {
            var expectedSites = _context.WorkSites.ToList();

            var result = _controller.List();

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedSites);
        }
        #endregion
    }
}
