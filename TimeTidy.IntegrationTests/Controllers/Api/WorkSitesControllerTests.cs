using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using TimeTidy.Controllers.Api;
using TimeTidy.Models;
using TimeTidy.Persistence;
using Moq;
using AutoMapper;

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
        }
    }
}
