using FirstLab.src.back_end;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class FlashcardDesignTest
    {
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { true, true, 5, 6, true };
            yield return new object[] { false, false, 3, 2, false };
            yield return new object[] { false, true, 5, 6, false };
            yield return new object[] { true, false, 5, 6, false };
            yield return new object[] { true, true, 4, 6, false };
            yield return new object[] { true, true, 5, 7, false };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void FlashcardDesign_Constructor_SetsPropertiesCorrectly(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize, bool expected)
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
            Assert.Equal(result, expected);
        }
    }
}
