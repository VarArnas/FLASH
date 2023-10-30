using FirstLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xunit;

namespace FirstLabTesting
{
    public class FlashcardSetTest
    {
        private readonly FlashcardSet flashcardSet;

        public FlashcardSetTest()
        {
            flashcardSet = new FlashcardSet();
        }

        
        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        public void FlashcardSetName_AssigningValueCorrectly_ReturnsSameString(string input, string expected)
        {
            // Arrange
            flashcardSet.FlashcardSetName = input;

            // Act
            var result = flashcardSet.FlashcardSetName;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FlashcardSetName_HandlesNull_ReturnsNull()
        {
            // Arrange

            // Act
            var result = flashcardSet.FlashcardSetName;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("Set1", "Set1")]
        [InlineData("new Set", "new Set")]
        [InlineData("", "")]

        public void Equal_ComparingEqualStrings_ReturnsTrue(string first, string second)
        {
            // Arrange
            FlashcardSet flashcardSet1 = new FlashcardSet { FlashcardSetName = first};
            FlashcardSet flashcardset2 = new FlashcardSet { FlashcardSetName = second};

            // Act
            var result = flashcardSet1.Equals(flashcardset2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equal_ComparingDifferentStrings_ReturnsFalse()
        {
            // Arrange
            FlashcardSet flashcardSet1 = new FlashcardSet { FlashcardSetName = "Set1" };
            FlashcardSet flashcardset2 = new FlashcardSet { FlashcardSetName = "Set2" };

            // Act
            var result = flashcardSet1.Equals(flashcardset2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equal_HandlesNull_ReturnsFalse()
        {
            // Arrange 
            FlashcardSet flashcardSet = new FlashcardSet { FlashcardSetName = "Set" };

            // Act 
            var result = flashcardSet.Equals(null);

            // Assert
            Assert.False(result);
        }

    }
}
