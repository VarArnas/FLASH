using FirstLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class FlashcardTest
    {
        private readonly Flashcard flashcard;
        public FlashcardTest() 
        {
            flashcard = new Flashcard();
        }

        [Theory]
        [InlineData ("test", "test")]
        [InlineData ("test with spaces", "test with spaces")]
        [InlineData ("", "")]
        [InlineData (null, null)]
        public void Flashcard_FlashcardQuestion_ReturnString(string input, string expected)
        {
            // Arrange
            
            // Act
            flashcard.FlashcardQuestion = input;
            var result = flashcard.FlashcardQuestion;
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Flashcard_FlashcardAnswer_ReturnString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardQuestion = input;
            var result = flashcard.FlashcardQuestion;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Flashcard_FlashcardName_ReturnString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardQuestion = input;
            var result = flashcard.FlashcardQuestion;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Flashcard_FlashcardColor_ReturnString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardQuestion = input;
            var result = flashcard.FlashcardQuestion;

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
