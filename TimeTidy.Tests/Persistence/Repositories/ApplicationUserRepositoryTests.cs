using System;
using NUnit.Framework;
using Moq;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;
using TimeTidy.Tests.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace TimeTidy.Tests.Persistence.Repositories
{
    [TestFixture]
    public class ApplicationUserRepositoryTests
    {
        private Mock<DbSet<IdentityRole>> _mockRoles;
        private Mock<DbSet<ApplicationUser>> _mockUsers;
        private Mock<ApplicationUserManager> _mockUserManger;
        private ApplicationUserRepository _repository;
        private ApplicationUser _user1;
        private ApplicationUser _user2;

        [SetUp]
        public void Setup()
        {
            _mockUserManger = new Mock<ApplicationUserManager>(new Mock<IUserStore<ApplicationUser>>().Object);
            _mockRoles = new Mock<DbSet<IdentityRole>>();
            _mockUsers = new Mock<DbSet<ApplicationUser>>();

            // Put SetSource here becuase it seems I can't set it after _mockRoles[Users] been passed via SetupGet.
            var role1 = new IdentityRole() { Id = "id1", Name = "role1" };
            var role2 = new IdentityRole() { Id = "id2", Name = "role2" };
            _mockRoles.SetSource(new[] { role1, role2 });

            _user1 = new ApplicationUser
            {
                Id = "user1",
                FirstName = "User1First",
                LastName = "User1Last",
                UserName = "User1UserName",
                Roles = { new IdentityUserRole { RoleId = role1.Id } }
            };

            _user2 = new ApplicationUser
            {
                Id = "user2",
                FirstName = "User2First",
                LastName = "User2Last",
                UserName = "User2UserName",
                Roles = { new IdentityUserRole { RoleId = role2.Id } }
            };

            _mockUsers.SetSource(new[] { _user1, _user2 });

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Roles).Returns(_mockRoles.Object);
            mockContext.SetupGet(c => c.Users).Returns(_mockUsers.Object);

            _repository = new ApplicationUserRepository(mockContext.Object, _mockUserManger.Object);
        }

        #region GetRoles()
        [Test]
        public void GetRoles_ValidCall_ShouldReturnIListOfRoleNames()
        {
            List<string> expected = new List<string>() { "role1", "role2" };

            var roles = _repository.GetRoles();

            roles.ShouldBeEquivalentTo(expected);
        }
        #endregion

        #region GetRolesForUser(id)
        [Test]
        public void GetRolesForUser_GivenUserFoundRolesReturned_ShouldReturnIListOfRoleNames()
        {
            List<string> expected = new List<string>() { "role1" };

            var roles = _repository.GetRolesForUser(_user1.Id);

            roles.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void GetRolesForUser_GivenUserDoesntHaveRoles_ShouldReturnNull()
        {
            var roles = _repository.GetRolesForUser(_user1.Id + "-");

            roles.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetRolesForUser_GivenUserNotFound_ShouldReturnNull()
        {
            var roles = _repository.GetRolesForUser(_user1.Id + "-");

            roles.Should().BeNull();
        }
        #endregion

        #region GetUser(id)
        [Test]
        public void GetUser_UserFoundWithGivenId_ShouldReturnApplicationUserOfGivenId()
        {
            var user = _repository.GetUser("user2");

            user.ShouldBeEquivalentTo(_user2);
        }

        [Test]
        public void GetUser_UserNotFound_ShouldReturnNull()
        {
            var user = _repository.GetUser(_user1.Id + "-");

            user.Should().BeNull();
        }
        #endregion

        #region GetUsers()
        [Test]
        public void GetUsers_ValidRequest_ShouldReturnEnumerableOfApplicatoinUser()
        {
            var expectedUsers = new[] { _user1, _user2 };

            var users = _repository.GetUsers();

            users.ShouldBeEquivalentTo(expectedUsers);
        }
        #endregion
    }
}
