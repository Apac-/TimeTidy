using System;
using NUnit.Framework;
using TimeTidy.Controllers;
using Moq;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using System.Collections.Generic;
using TimeTidy.Models;
using TimeTidy.Extensions;
using FluentAssertions;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeTidy.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController _controller;
        private Mock<IApplicationUserRepository> _mockApplicationUserRepo;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockApplicationUserRepo = new Mock<IApplicationUserRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Users).Returns(_mockApplicationUserRepo.Object);

            _controller = new UsersController(mockUoW.Object);

            _userId = "1";

            _controller.MockCurrentUser(_userId, "user@domain.com");
        }

        #region Index()
        [Test]
        public void Index_UsersFoundAndReturnedWithIndexView_ShouldReturnViewResultWithIndexViewAndEnumerableOfAppUser()
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = _userId,
                FirstName = "fName",
                LastName = "lName"
            };

            List<ApplicationUser> expected = new List<ApplicationUser>();
            expected.Add(user);

            _mockApplicationUserRepo.Setup(r => r.GetUsers()).Returns(expected);

            var result = _controller.Index();

            result.Should().BeOfType<ViewResult>().Which.ViewName.ShouldBeEquivalentTo("Index");
            result.Should().BeOfType<ViewResult>().Which.Model.ShouldBeEquivalentTo(expected);
        }
        #endregion

        #region Edit(id)
        [Test]
        public void Edit_UserNotFoundForGivenId_ShouldReturnHttpNotFoundResult()
        {
            _mockApplicationUserRepo.Setup(r => r.GetUser(_userId)).Returns(null as ApplicationUser);

            var result = _controller.Edit(_userId);

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Edit_UserFound_ShouldReturnViewResultWithUserFormViewAndUserFormViewModel()
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = _userId,
                FirstName = "fName",
                LastName = "lName",
                PhoneNumber = "555",
                Email = "user@domain.com",
            };

            List<String> roles = new List<string> { "admin", "manager", "user" };

            List<String> userRoles = new List<string> { "manager", "user" };

            UserFormViewModel expectedVM = new UserFormViewModel(user, roles, userRoles);

            _mockApplicationUserRepo.Setup(r => r.GetUser(_userId)).Returns(user);
            _mockApplicationUserRepo.Setup(r => r.GetRoles()).Returns(roles);
            _mockApplicationUserRepo.Setup(r => r.GetRolesForUser(_userId)).Returns(userRoles);

            var result = _controller.Edit(_userId);

            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("UserForm");
            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedVM);
        }
        #endregion

        #region Save(user)
        [Test]
        public void Save_ModelStateIsInvalidAndUserIsNotFoundWithGivenId_ShouldReturnHttpNotFound()
        {
            User user = new User
            {
                UserId = _userId,
                FirstName = "fName",
                LastName = "lName",
                Email = "user@domain.com",
            };

            _mockApplicationUserRepo.Setup(r => r.GetUser(_userId)).Returns(null as ApplicationUser);

            _controller.ModelState.AddModelError("error", "error");

            var taskResult = _controller.Save(user);

            taskResult.Should().BeOfType<Task<ActionResult>>();
            taskResult.Result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Save_ModelStateIsInvalid_ShouldReturnTaskViewResultWithViewUserFormAndUserFormViewModel()
        {
            ApplicationUser appUser = new ApplicationUser()
            {
                Id = _userId,
                FirstName = "fName",
                LastName = "lName",
                PhoneNumber = "555",
                Email = "user@domain.com",
            };

            User user = new User
            {
                UserId = _userId,
                FirstName = "fName",
                LastName = "lName",
                Email = "user@domain.com",
            };

            List<String> roles = new List<string> { "admin", "manager", "user" };

            List<String> userRoles = new List<string> { "manager", "user" };

            UserFormViewModel expectedVM = new UserFormViewModel(appUser, roles, userRoles);

            _mockApplicationUserRepo.Setup(r => r.GetUser(_userId)).Returns(appUser);
            _mockApplicationUserRepo.Setup(r => r.GetRoles()).Returns(roles);
            _mockApplicationUserRepo.Setup(r => r.GetRolesForUser(_userId)).Returns(userRoles);

            _controller.ModelState.AddModelError("error", "error");

            var taskResult = _controller.Save(user);

            taskResult.Should().BeOfType<Task<ActionResult>>();
            taskResult.Result.Should().BeOfType<ViewResult>()
                .Which.ViewName.ShouldBeEquivalentTo("UserForm");
            taskResult.Result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedVM);
        }

        [Test]
        public void Save_UserNotFoundWithGivenId_ShouldReturnTaskHttpNotFoundResult()
        {
            User user = new User
            {
                UserId = _userId,
                FirstName = "fName",
                LastName = "lName",
                Email = "user@domain.com",
            };

            _mockApplicationUserRepo.Setup(r => r.GetUser(_userId)).Returns(null as ApplicationUser);

            var taskResult = _controller.Save(user);

            taskResult.Should().BeOfType<Task<ActionResult>>();
            taskResult.Result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Save_UserSaveSuccessful_ShouldReturnRedirectToRouteResult()
        {
            ApplicationUser appUser = new ApplicationUser()
            {
                Id = _userId,
                FirstName = "fName",
                LastName = "lName",
                PhoneNumber = "555",
                Email = "user@domain.com",
            };

            var mockAppUser = new Mock<ApplicationUser>();
            mockAppUser.Setup(u => u.Roles).Returns(null as ICollection<IdentityUserRole>);

            User user = new User
            {
                UserId = _userId,
                FirstName = "fName",
                LastName = "lName",
                PhoneNumber = "444",
                Email = "user@domain.com",
                UserRoles = null
            };

            _mockApplicationUserRepo.Setup(r => r.GetUser(_userId)).Returns(appUser);

            var taskResult = _controller.Save(user);

            taskResult.Should().BeOfType<Task<ActionResult>>();
            taskResult.Result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Action"].ShouldBeEquivalentTo("Index");
            taskResult.Result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Controller"].ShouldBeEquivalentTo("Users");
        }
        #endregion
    }
}
