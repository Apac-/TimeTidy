using FluentAssertions;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeTidy.Controllers;
using TimeTidy.Models;
using TimeTidy.Persistence;

namespace TimeTidy.IntegrationTests.Controllers
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

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region Index()
        [Test]
        public void Index_ValidCall_ShouldReturnViewResultWithAllUsersInDb()
        {
            var expected = _context.Users.ToList();

            var result = _controller.Index();

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expected);
            result.As<ViewResult>().ViewName.ShouldBeEquivalentTo("Index");
        }
        #endregion

        #region Edit(id)
        [Test]
        public void Edit_UserIdNotFoundInDb_ShouldReturnHttpNotFoundResult()
        {
            var result = _controller.Edit("Not in Db");

            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [Test]
        public void Edit_UserFound_ShouldReturnUserFormViewModel()
        {
            var user = _context.Users.First();

            var roles = _context.Roles.Select(r => r.Name).ToArray();

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToArray();
            var userRoles = _context.Roles
                .Where(r => userRoleIds.Any(u => r.Id == u))
                .Select(r => r.Name)
                .ToList();

            var expectedViewModel = new UserFormViewModel(user, roles, userRoles);

            var result = _controller.Edit(user.Id);

            result.Should().BeOfType<ViewResult>()
                .Which.Model.ShouldBeEquivalentTo(expectedViewModel);
        }
        #endregion

        #region Save(user)
        [Test, Isolated]
        public async Task Save_UserIsUpdatedInDb_ShouldReturnRedirectAndSaveNewUserInfoIntoDb()
        {
            // Arrange
            var userInDb = _context.Users.First();

            var rolesInDb = _context.Roles.Select(r => r.Name).ToArray();

            User updatedUser = new User
            {
                UserId = userInDb.Id,
                Email = userInDb.Email + "-",
                FirstName = userInDb.FirstName + "-",
                LastName = userInDb.LastName + "-",
                PhoneNumber = userInDb.PhoneNumber + "0808",
                UserRoles = new List<string>() { TimeTidy.Models.RoleName.CanManageUsers }
            };

            // Act
            var result = await _controller.Save(updatedUser);

            //Assert

            // Get now updated user from DB
            var resultUser = _context.Users.Single(u => u.Id == userInDb.Id);

            // Assert basics
            resultUser.Email.Should().Be(updatedUser.Email);
            resultUser.FirstName.Should().Be(updatedUser.FirstName);
            resultUser.LastName.Should().Be(updatedUser.LastName);
            resultUser.PhoneNumber.Should().Be(updatedUser.PhoneNumber);

            // Transform role ids to names
            var resultUserRoleIds = resultUser.Roles.Select(r => r.RoleId).ToArray();
            var resultUserRoles = _context.Roles
                .Where(r => resultUserRoleIds.Any(u => r.Id == u))
                .Select(r => r.Name)
                .ToList();
            // Assert updated user in db has correct roles
            resultUserRoles.ShouldBeEquivalentTo(updatedUser.UserRoles);

            // Assert return types are correct
            result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Action"].ShouldBeEquivalentTo("Index");
            result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues["Controller"].ShouldBeEquivalentTo("Users");
        }

        [Test]
        public void Save_UserNotFoundInDb_ShouldReturnNotFoundResult()
        {
            User user = new User
            {
                UserId = "NOT IN DB",
                Email = "-",
                FirstName = "-",
                LastName = "-",
                PhoneNumber = "8",
            };

            var taskResult = _controller.Save(user);

            taskResult.Should().BeOfType<Task<ActionResult>>();
            taskResult.Result.Should().BeOfType<HttpNotFoundResult>();
        }
        #endregion
    }
}
