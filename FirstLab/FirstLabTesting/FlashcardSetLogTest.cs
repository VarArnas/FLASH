using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstLab.Migrations;
using FirstLab.src.back_end;

namespace FirstLabTesting
{
    public class FlashcardSetLogTest
    {
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "Set2", new DateTime(2023, 9, 30), 100 };
            yield return new object[] { "Set2", new DateTime(2023, 10, 20), 50 };
            yield return new object[] { "Set1", new DateTime(2022, 3, 14), 50 };
            yield return new object[] { "Set1", new DateTime(2023, 10, 20), 80 };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Constructor_SettingPropertiesIncorrectly_ReturnsFalse(string name, DateTime date, int duration)
        {
            // Arrange
            FlashcardSetLog flashcardSetLog = new FlashcardSetLog("Set1", new DateTime(2023, 10, 20), 50);
            FlashcardSetLog other = new FlashcardSetLog(name, date, duration);

            // Act
            var result = (flashcardSetLog.PlayedSetsName.Equals(other.PlayedSetsName) &&
                flashcardSetLog.Date.Date == other.Date.Date &&
                flashcardSetLog.Duration == other.Duration)
                ? true : false;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Constructor_SettingPropertiesCorrectly_ReturnsTrue()
        {
            // Arrange
            FlashcardSetLog flashcardSetLog1 = new FlashcardSetLog("Set", new DateTime(2023, 10, 20), 50);
            FlashcardSetLog flashcardSetLog2 = new FlashcardSetLog("Set", new DateTime(2023, 10, 20), 50);

            // Act
            var result = (flashcardSetLog1.PlayedSetsName.Equals(flashcardSetLog2.PlayedSetsName) &&
               flashcardSetLog1.Date.Date == flashcardSetLog2.Date.Date &&
               flashcardSetLog1.Duration == flashcardSetLog2.Duration)
               ? true : false;

            // Assert
            Assert.True(result);
        }
    }
}
