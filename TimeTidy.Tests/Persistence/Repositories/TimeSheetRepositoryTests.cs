using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using TimeTidy.Models;
using TimeTidy.Persistence;
using TimeTidy.Persistence.Repositories;
using TimeTidy.Tests.Extensions;

namespace TimeTidy.Tests.Persistence.Repositories
{
	[TestFixture]
	public class TimeSheetRepositoryTests
	{
        private Mock<DbSet<TimeSheet>> _mockTimeSheets;
        private TimeSheetRepository _repository;
        private TimeSheet _sheetOne;
        private TimeSheet _sheetTwo;
        private TimeSheet _sheetLoggedOn;
        private TimeSheet _sheetLoggedOnOlder;

        [SetUp]
		public void Setup()
		{
            _sheetOne = new TimeSheet
            {
                Id = 1,
                ApplicationUserId = "user1",
                WorkSiteId = 1,
                SiteName = "Site Name",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = new System.DateTime(1),
                LogOnLocation = new LatLng(1.0f, 1.0f),
                LogOffTime = new System.DateTime(2),
                LogOffLocation = new LatLng(1.0f, 1.0f),
            };

            _sheetTwo = new TimeSheet
            {
                Id = 2,
                ApplicationUserId = "user2",
                WorkSiteId = 2,
                SiteName = "Site Name",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = new System.DateTime(2),
                LogOnLocation = new LatLng(1.0f, 1.0f),
                LogOffTime = new System.DateTime(3),
                LogOffLocation = new LatLng(1.0f, 1.0f),
            };

            _sheetLoggedOn = new TimeSheet
            {
                Id = 3,
                ApplicationUserId = "user2",
                WorkSiteId = 2,
                SiteName = "Site Name",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = new System.DateTime(2),
                LogOnLocation = new LatLng(1.0f, 1.0f),
            };

            _sheetLoggedOnOlder = new TimeSheet
            {
                Id = 4,
                ApplicationUserId = "user2",
                WorkSiteId = 2,
                SiteName = "Site Name",
                SiteLocation = new LatLng(1.0f, 1.0f),
                LogOnTime = new System.DateTime(1), // Logged on before _sheetLoggedOn
                LogOnLocation = new LatLng(1.0f, 1.0f),
            };

            _mockTimeSheets = new Mock<DbSet<TimeSheet>>();
            _mockTimeSheets.SetSource(new[] { _sheetOne, _sheetTwo, _sheetLoggedOn, _sheetLoggedOnOlder });

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.TimeSheets).Returns(_mockTimeSheets.Object);

            _repository = new TimeSheetRepository(mockContext.Object);
		}

        #region GetTimeSheet(id)
        [Test]
        public void GetTimeSheet_TimeSheetForGivenIdNotFound_ShouldReturnNull()
        {
            var result = _repository.GetTimeSheet(999);

            result.Should().BeNull();
        }

        [Test]
        public void GetTimeSheet_TimeSheetFoundForGivenId_ShouldReturnTimeSheet()
        {
            var result = _repository.GetTimeSheet(_sheetOne.Id);

            result.ShouldBeEquivalentTo(result);
        }
        #endregion

        #region GetTimeSheets()
        [Test]
        public void GetTimeSheets_ValidRequest_ShouldReturnAllAvilableTimeSheets()
        {
            List<TimeSheet> expected = new List<TimeSheet>() { _sheetOne, _sheetTwo, _sheetLoggedOn, _sheetLoggedOnOlder };

            var result = _repository.GetTimeSheets();

            result.ShouldBeEquivalentTo(expected);
        }
        #endregion

        #region GetTimeSheetsByUser(userId)
        [Test]
        public void GetTimeSheetsByUser_NoSheetsWithGivenUserId_ShouldReturnNull()
        {
            var result = _repository.GetTimeSheetsByUser(_sheetOne.ApplicationUserId + "-");

            result.Should().BeEmpty();
        }

        [Test]
        public void GetTimeSheetsByUser_SheetsFoundWithGivenUserId_ShouldReturnListOfSheetsWithMatchingUserId()
        {
            List<TimeSheet> expected = new List<TimeSheet>() { _sheetOne };

            var result = _repository.GetTimeSheetsByUser(_sheetOne.ApplicationUserId);

            result.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void GetTimeSheetsByUser_SheetsFoundForUserAndIncludesSiteLocation_ShouldReturnListOfFoundSheets()
        {

            List<TimeSheet> expected = new List<TimeSheet>() { _sheetOne };

            var result = _repository.GetTimeSheetsByUser(_sheetOne.ApplicationUserId);

            result.Should().Contain(_sheetOne).Which.SiteLocation.ShouldBeEquivalentTo(_sheetOne.SiteLocation);
        }

        [Test]
        public void GetTimeSheetsByUser_SheetsFoundForUserAndIncludesLogOnLocation_ShouldReturnListOfFoundSheets()
        {

            List<TimeSheet> expected = new List<TimeSheet>() { _sheetOne };

            var result = _repository.GetTimeSheetsByUser(_sheetOne.ApplicationUserId);

            result.Should().Contain(_sheetOne).Which.SiteLocation.ShouldBeEquivalentTo(_sheetOne.LogOnLocation);
        }

        [Test]
        public void GetTimeSheetsByUser_SheetsFoundForUserAndIncludesLogOffLocation_ShouldReturnListOfFoundSheets()
        {

            List<TimeSheet> expected = new List<TimeSheet>() { _sheetOne };

            var result = _repository.GetTimeSheetsByUser(_sheetOne.ApplicationUserId);

            result.Should().Contain(_sheetOne).Which.SiteLocation.ShouldBeEquivalentTo(_sheetOne.LogOffLocation);
        }
        #endregion

        #region GetTimeSheetsByWorkSite(id)
        [Test]
        public void GetTimeSheetsByWorkSite_NoSheetsFoundWithGivenId_ShouldReturnEmpty()
        {
            var result = _repository.GetTimeSheetsByWorkSite(999);

            result.Should().BeEmpty();
        }

        [Test]
        public void GetTimeSheetsByWorkSite_SheetsFoundWithGivenId_ShouldReturnListOfSheetsWithWorkSiteId()
        {
            var expected = new List<TimeSheet>() { _sheetOne };

            var result = _repository.GetTimeSheetsByWorkSite(_sheetOne.WorkSiteId);

            result.ShouldBeEquivalentTo(expected);
            result.Should().NotContain(_sheetTwo);
        }
        #endregion

        #region GetCurrentLoggedInSheetForUser(userId, ? worksiteId)
        [Test]
        public void GetCurrentLoggedInSheetForUser_NoLoggedOnSheetFoundForUser_ShouldReturnNull()
        {
            var result = _repository.GetCurrentLoggedInSheetForUser(_sheetOne.ApplicationUserId + "-");

            result.Should().BeNull();
        }

        [Test]
        public void GetCurrentLoggedInSheetForUser_NoLoggedOnSheetFoundForUserAtGivenWorkSite_ShouldReturnNull()
        {
            var result = _repository.GetCurrentLoggedInSheetForUser(_sheetOne.ApplicationUserId, 999);

            result.Should().BeNull();
        }

        [Test]
        public void GetCurrentLoggedInSheetForUser_LoggedInSheetFoundForUser_ShouldReturnNewestFoundSheet()
        {
            var result = _repository.GetCurrentLoggedInSheetForUser(_sheetLoggedOn.ApplicationUserId);

            result.ShouldBeEquivalentTo(_sheetLoggedOn);
            result.Should().NotBe(_sheetLoggedOnOlder);
        }

        [Test]
        public void GetCurrentLoggedInSheetForUser_LoggedInSheetAvilableButNotOfGivenWorkSite_ShouldReutrnNull()
        {
            var result = _repository.GetCurrentLoggedInSheetForUser(_sheetLoggedOn.ApplicationUserId, _sheetLoggedOn.WorkSiteId + 1);

            result.Should().BeNull();
        }
        #endregion
    }
}
