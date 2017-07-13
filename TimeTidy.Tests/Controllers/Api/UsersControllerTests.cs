using NUnit.Framework;
using Moq;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Persistence;
using TimeTidy.Controllers.Api;
using TimeTidy.Models;
using TimeTidy.Extensions;
using System.Collections.Generic;
using FluentAssertions;
using System.Web.Http.Results;
using TimeTidy.Models.DTOs;

namespace TimeTidy.Tests.Controllers.Api
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController _controller;
        private Mock<IApplicationUserRepository> _mockUsersRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockUsersRepo = new Mock<IApplicationUserRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Users).Returns(_mockUsersRepo.Object);

            _userId = "1";

            _controller = new UsersController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain.com");
        }

        #region GetUsers
        [Test]
        public void GetUsers_ValidRequest_ShouldReturnOkResultWithListOfBasicUserDTO()
        {
            var foundUserOne = new ApplicationUser()
            {
                UserName = "first@user",
                Id = "11",
                FirstName = "user1",
                LastName = "user1last"
            };

            var foundUserTwo = new ApplicationUser()
            {
                UserName = "second@user",
                Id = "22",
                FirstName = "user2",
                LastName = "user2last"
            };

            List<ApplicationUser> foundUsers = new List<ApplicationUser>();
            foundUsers.Add(foundUserOne);
            foundUsers.Add(foundUserTwo);

            List<BasicUsersDTO> expectedResult = new List<BasicUsersDTO>();
            foreach (var user in foundUsers)
            {
                BasicUsersDTO newUser = new BasicUsersDTO
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                expectedResult.Add(newUser);
            }

            _mockUsersRepo.Setup(r => r.GetUsers()).Returns(foundUsers);

            var result = _controller.GetUsers();

            result.Should().BeOfType<OkNegotiatedContentResult<List<BasicUsersDTO>>>()
                .Which.Content.ShouldBeEquivalentTo(expectedResult);
        }
        #endregion

        #region GetUsers(id)
        [Test]
        public void GetUsers_ValidRequest_ShouldReturnOkResultWithApplicationUser()
        {
            var user = new ApplicationUser()
            {
                UserName = "username",
                Id = "id",
                FirstName = "firstName",
                LastName = "lastName"
            };

            _mockUsersRepo.Setup(r => r.GetUser(_userId)).Returns(user);

            var result = _controller.GetUsers(_userId);

            result.Should().BeOfType<OkNegotiatedContentResult<ApplicationUser>>()
                .Which.Content.Should().Be(user);
        }

        [Test]
        public void GetUsers_RequestForNonExistantUser_ShouldReturnNotFoundResult()
        {
            _mockUsersRepo.Setup(r => r.GetUser("fake")).Returns(null as ApplicationUser);

            var result = _controller.GetUsers("fake");

            result.Should().BeOfType<NotFoundResult>();
        }
        #endregion

        #region DeleteUser
        [Test]
        public void DeleteUser_ValidRequest_ShouldReturnWithOkResult()
        {
            var user = new ApplicationUser()
            {
                UserName = "username",
                Id = "id",
                FirstName = "firstName",
                LastName = "lastName"
            };

            _mockUsersRepo.Setup(r => r.GetUser(_userId)).Returns(user);

            var result = _controller.DeleteUser(_userId);

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public void DeleteUser_RequestForNonExistantUser_ShouldReturnNotFoundResult()
        {
            _mockUsersRepo.Setup(r => r.GetUser("fake")).Returns(null as ApplicationUser);

            var result = _controller.DeleteUser("fake");

            result.Should().BeOfType<NotFoundResult>();
        }
        #endregion
    }
}
