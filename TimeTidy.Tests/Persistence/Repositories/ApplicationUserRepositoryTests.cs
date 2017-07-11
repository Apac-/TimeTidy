using System;
using NUnit.Framework;
using Moq;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;

namespace TimeTidy.Tests.Persistence.Repositories
{
    [TestFixture]
    public class ApplicationUserRepositoryTests
    {
        private Mock<DbSet<IdentityRole>> _mockRoles;
        private Mock<DbSet<ApplicationUser>> _mockUsers;
        private Mock<ApplicationUserManager> _mockUserManger;
        private ApplicationUserRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockUserManger = new Mock<ApplicationUserManager>();
            _mockRoles = new Mock<DbSet<IdentityRole>>();
            _mockUsers = new Mock<DbSet<ApplicationUser>>();

            var mockContext = new Mock<IApplicationDbContext>();

            _repository = new ApplicationUserRepository(mockContext.Object, _mockUserManger.Object);
        }
    }
}
