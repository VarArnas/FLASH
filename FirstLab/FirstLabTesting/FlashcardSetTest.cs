using FirstLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [InlineData(null, null)]
        public void FlashcardSet_FlashcardSetName_ReturnString(string input, string expected)
        {
            // Arrange
            flashcardSet.FlashcardSetName = input;

            // Act
            var result = flashcardSet.FlashcardSetName;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("Set1", true)]
        [InlineData("Set2", false)]
        public void FlashcardSet_Equal_ReturnBoolean(string setName, bool expected)
        {
            // Arrange
            flashcardSet.FlashcardSetName = "Set1";
            FlashcardSet? other = setName != null ? new FlashcardSet { FlashcardSetName = setName } : null; 

            // Act
            var result = flashcardSet.Equals(other);

            // Assert
            Assert.Equal(expected, result);
        }


    }
}
