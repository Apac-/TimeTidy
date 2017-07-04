using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using TimeTidy.Persistence;
using FluentAssertions;
using TimeTidy.Controllers.Api;
using TimeTidy.Tests.Extensions;
using TimeTidy.Persistence.Repositories;
using System.Web.Http.Results;

namespace TimeTidy.Tests.Controllers.Api
{
    [TestFixture]
    public class TimeSheetsControllerTests
    {
        private TimeSheetsController _controller;
        private Mock<ITimeSheetRepository> _mockTimeSheetRepo;

        [SetUp]
        public void Setup()
        {
            _mockTimeSheetRepo = new Mock<ITimeSheetRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.TimeSheets).Returns(_mockTimeSheetRepo.Object);

            _controller = new TimeSheetsController(mockUoW.Object);
            _controller.MockCurrentUser("1", "user@domain.com");
        }

        [Test]
        public void UpdateTimeSheet_NoSheetWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.UpdateTimeSheet(80, new Models.DTOs.TimeSheetLogoffDTO());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
