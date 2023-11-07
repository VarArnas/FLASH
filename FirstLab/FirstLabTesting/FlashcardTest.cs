using FirstLab;
using FirstLab.src.models;
using Moq;
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
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        public void FlashcardQuestion_AssigningCorrectValue_ReturnsSameString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardQuestion = input;
            var result = flashcard.FlashcardQuestion;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FlashcardQuestion_HandlesNull_ReturnsNull() 
        {
            // Arrange

            // Act
            flashcard.FlashcardQuestion = null;
            var result = flashcard.FlashcardQuestion;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        public void FlashcardAnswer_AssigningCorrectValue_ReturnsSameString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardAnswer = input;
            var result = flashcard.FlashcardAnswer;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FlashcardAnswer_HandlesNull_ReturnsNull()
        {
            // Arrange

            // Act
            flashcard.FlashcardAnswer = null;
            var result = flashcard.FlashcardAnswer;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        public void FlashcardColor_AssigningCorrectValue_ReturnsSameString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardColor = input;
            var result = flashcard.FlashcardColor;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FlashcardColor_HandlesNull_ReturnsNull()
        {
            // Arrange

            // Act
            flashcard.FlashcardColor = null;
            var result = flashcard.FlashcardColor;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test with spaces", "test with spaces")]
        [InlineData("", "")]
        public void FlashcardName_AssigningCorrectValue_ReturnsSameString(string input, string expected)
        {
            // Arrange

            // Act
            flashcard.FlashcardName = input;
            var result = flashcard.FlashcardName;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FlashcardName_HandlesNull_ReturnsNull()
        {
            // Arrange

            // Act
            var result = flashcard.FlashcardName;

            // Assert
            Assert.Null(result);
        }

    }
}
