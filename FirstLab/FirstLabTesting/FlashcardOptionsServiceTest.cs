using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.services;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace FirstLabTesting
{
    public class FlashcardOptionsServiceTest
    {
        private FlashcardOptionsService service;
        public FlashcardOptionsServiceTest()
        {
            var mockFactoryContainer = new Mock<IFactoryContainer>();
            var mockFlashcardSetMapper = new Mock<IFlashcardSetMapper>();
            var mockDatabaseRepository = new Mock<IDatabaseRepository>();

            service = new FlashcardOptionsService(
                mockFactoryContainer.Object,
                mockFlashcardSetMapper.Object,
                mockDatabaseRepository.Object
            );
        }

        [Theory]
        [InlineData ("IndianRed", "Very easy")]
        [InlineData ("Green", "Easy")]
        [InlineData ("Yellow", "Medium")]
        [InlineData ("RoyalBlue", "Hard")]
        [InlineData ("Orange", "Very Hard")]
        public void CalculateDifficultyOfFlashcardSet_PassingSingleFlashcard_ReturnsAppropriateDifficultyString(string input, string expectedResult)
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet();
            Flashcard flashcard = new Flashcard { FlashcardColor = input};
            flashcardSet.Flashcards!.Add(flashcard);

            // Act
            string result = service.CalculateDifficultyOfFlashcardSet(flashcardSet);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData ("IndianRed", "IndianRed", "Very easy")]
        [InlineData ("Yellow", "Orange", "Hard")]
        public void CalculateDifficultyOfFlashcardSet_PassingMultipleFlashcards_ReturnsAppropriateDifficultyString(string input1, string input2, string expectedResult)
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet();
            Flashcard flashcard1 = new Flashcard { FlashcardColor = input1 };
            Flashcard flashcard2 = new Flashcard { FlashcardColor = input2 };
            flashcardSet.Flashcards!.Add(flashcard1);
            flashcardSet.Flashcards.Add(flashcard2);

            // Act
            string result = service.CalculateDifficultyOfFlashcardSet(flashcardSet);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculateDifficultyOfFlashcardSet_PassingUnexpectedStrings_ReturnsDefaultDifficultyString()
        {
            // Arrange
            string unexpectedString = "word";
            FlashcardSet flashcardSet = new FlashcardSet();
            Flashcard flashcard = new Flashcard { FlashcardColor =  unexpectedString};
            flashcardSet.Flashcards!.Add(flashcard);
            string expectedResult = "Medium";

            // Act
            string result = service.CalculateDifficultyOfFlashcardSet(flashcardSet);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculateDifficultyOfFlashcardSet_HandlingNull_ReturnsDefaultDifficultyString()
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet();
            Flashcard flashcard = new Flashcard { FlashcardColor = null };
            flashcardSet.Flashcards!.Add(flashcard);
            string expectedResult = "Medium";

            // Act
            string result = service.CalculateDifficultyOfFlashcardSet(flashcardSet);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
