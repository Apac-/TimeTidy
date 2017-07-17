using System;
using NUnit.Framework;
using Moq;
using FluentAssertions;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Persistence;
using TimeTidy.Controllers.Api;
using System.Collections.Generic;
using TimeTidy.Models;
using TimeTidy.Extensions;
using System.Web.Http.Results;
using TimeTidy.DTOs;
using AutoMapper;
using System.Net.Http;
using System.Web.Http;

namespace TimeTidy.Tests.Controllers.Api
{
    [TestFixture]
    public class WorkSitesControllerTests
    {
        private WorkSitesController _controller;
        private Mock<IWorkSiteRepository> _mockWorkSitesRepo;
        private Mock<IMapper> _mockMapper;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockWorkSitesRepo = new Mock<IWorkSiteRepository>();

            _userId = "1";

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.WorkSites).Returns(_mockWorkSitesRepo.Object);

            _mockMapper = new Mock<IMapper>();

            _controller = new WorkSitesController(mockUoW.Object, _mockMapper.Object);
            _controller.MockCurrentUser(_userId, "user@domain.com");

            // Populate requests for CreateWorkSite(WorkSiteDTO)
            _controller.Request = new HttpRequestMessage();
            _controller.Request.SetConfiguration(new HttpConfiguration());
            _controller.Request.RequestUri = new Uri("http://domain.com");
        }

        #region GetWorkSites
        [Test]
        public void GetWorkSites_ValidRequest_ShouldReturnOkResultWithIEnumWorkSites()
        {
            WorkSite siteOne = new WorkSite()
            {
                Id = 1,
                Name = "site name",
                StreetAddress = "1 site street",
                Lat = 1.0f,
                Lng = 1.0f
            };

            List<WorkSite> expectedResult = new List<WorkSite>();
            expectedResult.Add(siteOne);

            _mockWorkSitesRepo.Setup(r => r.GetWorkSites()).Returns(expectedResult);

            var result = _controller.GetWorkSites();

            result.Should().BeOfType<OkNegotiatedContentResult<IEnumerable<WorkSite>>>()
                .Which.Content.ShouldBeEquivalentTo(expectedResult);
        }
        #endregion

        #region GetWorkSite(id)
        [Test]
        public void GetWorkSite_GivenIdNotFound_ShouldReturnNotFoundResult()
        {
            _mockWorkSitesRepo.Setup(r => r.GetWorkSite(1)).Returns(null as WorkSite);

            var result = _controller.GetWorkSite(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void GetWorkSite_ValidRequest_ShouldReturnOkWithFoundWorkSite()
        {
            WorkSite siteOne = new WorkSite()
            {
                Id = 1,
                Name = "site name",
                StreetAddress = "1 site street",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSitesRepo.Setup(r => r.GetWorkSite(1)).Returns(siteOne);

            var result = _controller.GetWorkSite(1);

            result.Should().BeOfType<OkNegotiatedContentResult<WorkSite>>()
                .Which.Content.ShouldBeEquivalentTo(siteOne);
        }
        #endregion

        #region CreateWorkSite(dto)
        [Test]
        public void CreateWorkSite_ValidRequest_ShouldReturnCreatedNegotiatedContentResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Id = 1,
                Name = "name",
                Lat = 1.0f,
                Lng = 1.0f
            };

            WorkSite mappedSite = new WorkSite
            {
                Id = dto.Id,
                Name = dto.Name,
                Lat = dto.Lat,
                Lng = dto.Lng,
            };

            _mockMapper.Setup(m => m.Map<WorkSiteDTO, WorkSite>(dto)).Returns(mappedSite);

            Uri expectedResultUri = new Uri(_controller.Request.RequestUri + "/" + dto.Id);

            var result = _controller.CreateWorkSite(dto);

            result.Should().BeOfType<CreatedNegotiatedContentResult<WorkSiteDTO>>()
                .Which.Content.ShouldBeEquivalentTo(dto);
            result.Should().BeOfType<CreatedNegotiatedContentResult<WorkSiteDTO>>()
                .Which.Location.Should().Be(expectedResultUri);
        }

        [Test]
        public void CreateWorkSite_ModelStateIsNotValid_ShouldReturnBadRequestResult()
        {
            _controller.ModelState.AddModelError("error", "error");

            WorkSiteDTO dto = new WorkSiteDTO();

            var result = _controller.CreateWorkSite(dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void CreateWorkSite_WorkSiteDTODoesntHaveRequiredFieldName_ShouldReturnBadRequestResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Lat = 1.0f,
                Lng = 1.0f
            };

            var result = _controller.CreateWorkSite(dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void CreateWorkSite_WorkSiteDTODoesntHaveRequiredFieldLat_ShouldReturnBadRequestResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Name = "name",
                Lng = 1.0f
            };

            var result = _controller.CreateWorkSite(dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void CreateWorkSite_WorkSiteDTODoesntHaveRequiredFieldLng_ShouldReturnBadRequestResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Name = "name",
                Lat = 1.0f,
            };

            var result = _controller.CreateWorkSite(dto);

            result.Should().BeOfType<BadRequestResult>();
        }
        #endregion

        #region UpdateWorkSite(id, dto)
        [Test]
        public void UpdateWorkSite_ModelStateIsNotValid_ShouldReturnBadRequestResult()
        {
            _controller.ModelState.AddModelError("error", "error");

            WorkSiteDTO dto = new WorkSiteDTO();

            var result = _controller.UpdateWorkSite(1, dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateWorkSite_SiteIdNotFound_ShouldReturnNotFoundResult()
        {
            _mockWorkSitesRepo.Setup(r => r.GetWorkSite(1)).Returns(null as WorkSite);

            WorkSiteDTO dto = new WorkSiteDTO
            {
                Name = "name",
                Lat = 1.0f,
                Lng = 1.0f
            };

            var result = _controller.UpdateWorkSite(1, dto);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void UpdateWorkSite_WorkSiteDtoDoesNotHaveRequiredFieldName_ShouldReturnBadRequestResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Lat = 1.0f,
                Lng = 1.0f
            };

            var result = _controller.UpdateWorkSite(1, dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateWorkSite_WorkSiteDtoDoesNotHaveRequiredFieldLat_ShouldReturnBadRequestResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Name = "name",
                Lng = 1.0f
            };

            var result = _controller.UpdateWorkSite(1, dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateWorkSite_WorkSiteDtoDoesNotHaveRequiredFieldLng_ShouldReturnBadRequestResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Name = "name",
                Lat = 1.0f
            };

            var result = _controller.UpdateWorkSite(1, dto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void UpdateWorkSite_ValidRequest_ShouldReturnOkResult()
        {
            WorkSiteDTO dto = new WorkSiteDTO
            {
                Name = "name",
                Lat = 1.0f,
                Lng = 1.0f
            };

            WorkSite expectedSite = new WorkSite
            {
                Name = dto.Name,
                StreetAddress = "address",
                Lat = dto.Lat,
                Lng = dto.Lng
            };

            _mockWorkSitesRepo.Setup(m => m.GetWorkSite(1)).Returns(expectedSite);
            _mockMapper.Setup(m => m.Map<WorkSite>(dto)).Returns(expectedSite);

            var result = _controller.UpdateWorkSite(1, dto);

            result.Should().BeOfType<OkResult>();
        }
        #endregion

        #region DeleteWorkSite(id)
        [Test]
        public void DeleteWorkSite_SiteWithGivenIdNotFound_ShouldReturnNotFoundResult()
        {
            _mockWorkSitesRepo.Setup(m => m.GetWorkSite(1)).Returns(null as WorkSite);

            var result = _controller.DeleteWorkSite(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void DeleteWorkSite_ValidRequest_ShouldReturnOkResult()
        {
            WorkSite site = new WorkSite
            {
                Name = "name",
                StreetAddress = "address",
                Lat = 1.0f,
                Lng = 1.0f
            };

            _mockWorkSitesRepo.Setup(m => m.GetWorkSite(1)).Returns(site);

            var result = _controller.DeleteWorkSite(1);

            result.Should().BeOfType<OkResult>();
        }
        #endregion
    }
}
