using Moq;
using NUnit.Framework;
using System.Data.Entity;
using TimeTidy.Models;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;

namespace TimeTidy.Tests.Persistence.Repositories
{
	[TestFixture]
	public class TimeSheetRepositoryTests
	{
        private Mock<DbSet<TimeSheet>> _mockTimeSheets;
        private Mock<TimeSheetRepository> _repository;

        [SetUp]
		public void Setup()
		{
            _mockTimeSheets = new Mock<DbSet<TimeSheet>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.TimeSheets).Returns(_mockTimeSheets.Object);

            _repository = new Mock<TimeSheetRepository>(mockContext.Object);
		}
	}
}
