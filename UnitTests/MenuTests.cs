using CodingTrackerConsoleApp;

namespace UnitTests
{
    [TestClass]
    public class MenuTests
    {
        [TestMethod]
        public void ParseDate_ValidInput_ReturnsCorrectDateTime()
        {
            // Arrange
            string input = "2023-10-01";
            DateTime expected = new(2023, 10, 01);

            // Act
            DateTime? result = Menu.ParseDate(input);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseDate_InvalidInput_ThrowsFormatException()
        {
            // Arrange
            string input = "invalid date";

            // Act
            Menu.ParseDate(input);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void ParseTime_ValidInput_ReturnsCorrectDateTime()
        {
            // Arrange
            string input = "14:30";
            DateTime expected = DateTime.Today.AddHours(14).AddMinutes(30);

            // Act
            DateTime? result = Menu.ParseTime(input);

            // Assert
            Assert.AreEqual(expected.Hour, result?.Hour);
            Assert.AreEqual(expected.Minute, result?.Minute);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseTime_InvalidInput_ThrowsFormatException()
        {
            // Arrange
            string input = "invalid time";

            // Act
            Menu.ParseTime(input);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseTime_InvalidTimeValue_ThrowsFormatException()
        {
            // Arrange
            string input = "25:61";

            // Act
            Menu.ParseTime(input);

            // Assert is handled by ExpectedException
        }
    }
}
