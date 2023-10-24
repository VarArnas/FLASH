using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstLab.src.back_end;

namespace FirstLabTesting
{
    public class FlashcardSetLogTest
    {
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "Set1", new DateTime(2023, 10, 20), 50, true};
            yield return new object[] { "Set2", new DateTime(2023, 9, 30), 100, false };
            yield return new object[] { "Set2", new DateTime(2023, 10, 20), 50, false };
            yield return new object[] { "Set1", new DateTime(2022, 3, 14), 50, false };
            yield return new object[] { "Set1", new DateTime(2023, 10, 20), 80, false };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void FlashcardSetLog_Constructor_SetsPropertiesCorrectly(string name, DateTime date, int duration, bool expected)
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
            Assert.Equal(expected, result);
        }
    }
}
