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
    public class UsersControllerTests
    {
        private ApplicationDbContext _context;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _controller = new UsersController(new UnitOfWork(_context, userManager));
        }

        #region GetUsers()
        [Test]
        public void GetUsers_ValidRequest_ReturnListOfUsersInBasicUsersDto()
        {
            var usersInDb = _context.Users.ToList();

            var expectedDto = usersInDb.Select(user => new BasicUsersDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            });

            var result = _controller.GetUsers();

            result.Should().BeOfType<OkNegotiatedContentResult<List<BasicUsersDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expectedDto);
        }
        #endregion

        #region GetUsers(id)
        [Test]
        public void GetUsers_UserWithGivenIdNotFoundInDb_ShouldReturnNotFoundResult()
        {
            var result = _controller.GetUsers("UserIdNotInDb");

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void GetUsers_UserFoundWithGivenID_ShouldReturnUser()
        {
            var user = _context.Users.First();

            var result = _controller.GetUsers(user.Id);

            result.Should().BeOfType<OkNegotiatedContentResult<ApplicationUser>>()
                .Which.Content.ShouldBeEquivalentTo(user);
        }
        #endregion

        #region DeleteUser(id)
        [Test, Isolated]
        public void DeleteUser_UserWithGivenIDNotFound_ShouldReturnNotFoundResult()
        {
            var result = _controller.GetUsers("UserIdNotInDb");

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Isolated]
        public void DeleteUser_UserFoundAndRemoved_ShouldRemoveUserFromDb()
        {
            var user = _context.Users.First();

            var result = _controller.DeleteUser(user.Id);

            result.Should().BeOfType<OkResult>();

            var userInDb = _context.Users.SingleOrDefault(c => c.Id == user.Id);
            userInDb.Should().BeNull();
        }
        #endregion
    }
}
