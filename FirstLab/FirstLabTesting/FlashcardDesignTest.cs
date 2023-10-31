using FirstLab.src.back_end;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FirstLabTesting
{
    public class FlashcardDesignTest
    {
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { false, false, 3, 2 };
            yield return new object[] { false, true, 5, 6 };
            yield return new object[] { true, false, 5, 6 };
            yield return new object[] { true, true, 4, 6 };
            yield return new object[] { true, true, 5, 7 };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Constructor_SettingPropertiesIncorrectly_ReturnsFalse(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize)
        {
            // Arrange
            FlashcardDesign flashcardDesign = new FlashcardDesign(true, true, 5, 6);
            FlashcardDesign other = new FlashcardDesign(isItalic, isHighlighted, increaseTextSize, decreaseTextSize);

            // Act
            var result = (flashcardDesign.IsItalic == other.IsItalic &&
                flashcardDesign.IsHighlighted == other.IsHighlighted &&
                flashcardDesign.IncreaseTextSize == other.IncreaseTextSize &&
                flashcardDesign.DecreaseTextSize == other.DecreaseTextSize)
                ? true : false;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Constructor_SettingPropertiesCorrectly_ReturnsTrue()
        {
            // Arrange
            FlashcardDesign flashcardDesign1 = new FlashcardDesign(true, true, 5, 6);
            FlashcardDesign flashcardDesign2 = new FlashcardDesign(true, true, 5, 6);

            // Act
            var result = (flashcardDesign1.IsItalic == flashcardDesign2.IsItalic &&
                flashcardDesign1.IsHighlighted == flashcardDesign2.IsHighlighted &&
                flashcardDesign1.IncreaseTextSize == flashcardDesign2.IncreaseTextSize &&
                flashcardDesign1.DecreaseTextSize == flashcardDesign2.DecreaseTextSize)
                ? true : false;

            // Assert
            Assert.True(result); 
        }
    }
}
