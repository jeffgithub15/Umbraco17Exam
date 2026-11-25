using Microsoft.Extensions.Options;
using Moq;
using UmbracoExam17.Domain.Services;
using UmbracoExam17.Models;

namespace UmbracoExam17.Tests
{

    [TestClass]
    public class ReadingTimeServiceTests
    {
        private ReadingTimeService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            var settings = new GlobalSetting
            {
                WordsPerMinute = 200
            };

            var optionsMonitor = Mock.Of<IOptionsMonitor<GlobalSetting>>(
                x => x.CurrentValue == settings
            );

            _service = new ReadingTimeService(optionsMonitor);
        }

        [TestMethod]
        public void CalculateReadingTime_NullContent_ReturnsZero()
        {
            var result = _service.CalculateReadingTime(null);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateReadingTime_EmptyContent_ReturnsZero()
        {
            var result = _service.CalculateReadingTime(string.Empty);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateReadingTime_WhitespaceContent_ReturnsZero()
        {
            var result = _service.CalculateReadingTime("   \t  \n ");

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateReadingTime_SingleWord_ReturnsOne()
        {
            var result = _service.CalculateReadingTime("Hello");

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CalculateReadingTime_MoreThanOneMinute_ReturnsTwo()
        {
            var content = string.Join(" ", Enumerable.Repeat("word", 201));

            var result = _service.CalculateReadingTime(content);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void CalculateReadingTime_ExactlyOneMinute_ReturnsOne()
        {
            var content = string.Join(" ", Enumerable.Repeat("word", 200));

            var result = _service.CalculateReadingTime(content);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CalculateReadingTime_MultipleSpaces_CountsCorrectly()
        {
            var result = _service.CalculateReadingTime("Hello    world   from   MSTest");

            Assert.AreEqual(1, result);
        }
    }
}
