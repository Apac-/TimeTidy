using Moq;
using NUnit.Framework;
using TimeTidy.Controllers;
using TimeTidy.Persistence;
using TimeTidy.Tests.Extensions;

namespace TimeTidy.Tests.Controllers
{
    [TestFixture]
    public class TimeSheetsControllerTests
    {
        private TimeSheetsController _controller;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            var mockUoW = new Mock<IUnitOfWork>();

            _userId = "1";

            _controller = new TimeSheetsController(mockUoW.Object);
            _controller.MockCurrentUser(_userId, "user@domain");
        }
    }
}
