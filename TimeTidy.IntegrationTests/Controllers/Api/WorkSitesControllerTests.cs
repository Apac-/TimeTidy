using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using TimeTidy.Controllers.Api;
using TimeTidy.Models;
using TimeTidy.Persistence;
using Moq;
using AutoMapper;
using System.Linq;
using FluentAssertions;
using System.Web.Http.Results;
using System.Collections.Generic;
using TimeTidy.DTOs;
using System.Net.Http;
using System.Web.Http;

namespace TimeTidy.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class WorkSitesControllerTests
    {
        private ApplicationDbContext _context;
        private WorkSitesController _controller;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext();

            _mockMapper = new Mock<IMapper>();

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _controller = new WorkSitesController(new UnitOfWork(_context, userManager), _mockMapper.Object);

            // Populate requests for CreateWorkSite(WorkSiteDTO)
            _controller.Request = new HttpRequestMessage();
            _controller.Request.SetConfiguration(new HttpConfiguration());
            _controller.Request.RequestUri = new Uri("http://domain.com");
        }

        #region GetWorkSites()
        [Test, Isolated]
        public void GetWorkSites_ValidRequest_ShouldReturnAllWorkSites()
        {
            var workSite = new WorkSite
            {
                Name = "WorkSite Name",
                Description = "Thing",
                StreetAddress = "Address",
                Lat = 2.0f,
                Lng = 2.0f,
            };

            var workSiteInDb = _context.WorkSites.Add(workSite);

            _context.SaveChanges();

            var expected = _context.WorkSites.ToList();

            var result = _controller.GetWorkSites();

            result.Should().BeOfType<OkNegotiatedContentResult<IEnumerable<WorkSite>>>()
                .Which.Content.ShouldBeEquivalentTo(expected);
        }
        #endregion

        #region GetWorkSite(id)
        [Test]
        public void GetWorkSite_WorkSiteWithGivenIdNotFound_ShouldReturnNotFoundResult()
        {
            var notRealId = 21341;

            var result = _controller.GetWorkSite(notRealId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void GetWorkSite_WorkSiteFound_ShouldReturnWorkSite()
        {
            var workSite = new WorkSite
            {
                Name = "WorkSite Name",
                Description = "Thing",
                StreetAddress = "Address",
                Lat = 2.0f,
                Lng = 2.0f,
            };

            var siteInDb = _context.WorkSites.Add(workSite);

            _context.SaveChanges();

            var result = _controller.GetWorkSite(siteInDb.Id);

            result.Should().BeOfType<OkNegotiatedContentResult<WorkSite>>()
                .Which.Content.ShouldBeEquivalentTo(siteInDb);
        }
        #endregion

        #region CreateWorkSite(WorkSiteDto)
        [Test, Isolated]
        public void CreateWorkSite_WorkSiteAddedToDbFromDto_ShouldAddNewlyCreatedWorkSiteToDB()
        {
            var dto = new WorkSiteDTO
            {
                Name = "A new site",
                Description = "New site",
                Lat = 5.0f,
                Lng = 7.0f,
            };

            var mappedSite = new WorkSite
            {
                Name = dto.Name,
                Description = dto.Description,
                StreetAddress = "Address",
                Lat = dto.Lat,
                Lng = dto.Lng,
            };

            _mockMapper.Setup(m => m.Map<WorkSiteDTO, WorkSite>(dto)).Returns(mappedSite);

            var result = _controller.CreateWorkSite(dto);

            _context.WorkSites.Should().Contain(c => c.Name == dto.Name && c.Lat == dto.Lat && c.Lng == dto.Lng);
        }
        #endregion

        #region UpdateWorkSite(id, WorkSiteDto)
        [Test, Isolated]
        public void UpdateWorkSite_SiteWithGivenIDNotFound_ShouldReturnNotFoundResult()
        {
            var dto = new WorkSiteDTO
            {
                Name = "A new site",
                Description = "New site",
                Lat = 5.0f,
                Lng = 7.0f,
            };

            var notRealId = 112351;

            var result = _controller.UpdateWorkSite(notRealId, dto);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void UpdateWorkSite_SiteUpdatedWithDto_ShouldUpdateWorkSiteInDbAndReturnOk()
        {
            var dto = new WorkSiteDTO
            {
                Name = "An updated site",
                Description = "Updated site",
                Lat = 25.7f,
                Lng = 18.45f,
            };

            var addedSite = _context.WorkSites.Add(new WorkSite
            {
                Name = "-",
                StreetAddress = "-",
                Lat = 1.0f,
                Lng = 1.0f
            });

            _context.SaveChanges();

            var mappedSite = new WorkSite
            {
                Id = addedSite.Id,
                Name = dto.Name,
                Description = dto.Description,
                Lat = dto.Lat,
                Lng = dto.Lng,
                StreetAddress = addedSite.StreetAddress,
            };

            // Act
            var result = _controller.UpdateWorkSite(addedSite.Id, dto);

            // Assert
            result.Should().BeOfType<OkResult>();

            var siteInDb = _context.WorkSites.Single(c => c.Id == addedSite.Id);
            siteInDb.Name.Should().Be(dto.Name);
            siteInDb.Description.Should().Be(dto.Description);
            siteInDb.Lat.Should().Be(dto.Lat);
            siteInDb.Lng.Should().Be(dto.Lng);
        }
        #endregion

        #region DeleteWorkSite(id)
        [Test, Isolated]
        public void DeleteWorkSite_WorkSiteWithGivenIdNotFoundInDb_ShouldReturnNotFoundResult()
        {
            var notInDb = 412514;

            var result = _controller.DeleteWorkSite(notInDb);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void DeleteWorkSite_WorkSiteFoundAndRemovedFromDb_ShouldRemovedSiteWithGivenIdFromDb()
        {
            var addedSite = _context.WorkSites.Add(new WorkSite
            {
                Name = "-",
                StreetAddress = "-",
                Lat = 1.0f,
                Lng = 1.0f
            });

            _context.SaveChanges();

            var result = _controller.DeleteWorkSite(addedSite.Id);

            result.Should().BeOfType<OkResult>();

            var siteInDb = _context.WorkSites.SingleOrDefault(c => c.Id == addedSite.Id);
            siteInDb.Should().BeNull();
        }
        #endregion
    }
}
