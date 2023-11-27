using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class FlashcardCustomizationServiceTest
    {
        private FlashcardCustomizationService service;
        private Mock<IFlashcardSetMapper> mockFlashcardSetMapper;
        private Mock<IFactoryContainer> mockFactoryContainer;
        private Mock<IDatabaseRepository> mockDatabaseRepository;
        public FlashcardCustomizationServiceTest()
        {
            mockFactoryContainer = new Mock<IFactoryContainer>();
            mockFlashcardSetMapper = new Mock<IFlashcardSetMapper>();
            mockDatabaseRepository = new Mock<IDatabaseRepository>();
            
            service = new FlashcardCustomizationService(
                mockFactoryContainer.Object, 
                mockFlashcardSetMapper.Object, 
                mockDatabaseRepository.Object
            );
        }

        [Fact]
        public void AddFlashcard_PassingFlashcardSet_AddsFlashcardToTheEnd()
        {
            // Arrange
            mockFactoryContainer.Setup(f => f.CreateObject<Flashcard>()).Returns(new Flashcard());
            FlashcardSet flashcardSet = new FlashcardSet();
            var flashcard1 = new Flashcard();
            var flashcard2 = new Flashcard();
            flashcardSet.Flashcards!.Add(flashcard1);
            flashcardSet.Flashcards.Add(flashcard2);

            // Act
            int index = service.AddFlashcard(flashcardSet);

            // Assert
            Assert.True(flashcardSet.Flashcards.Count == 3);
            Assert.Equal(2, index);
            Assert.NotEqual(flashcard1, flashcardSet.Flashcards[index]);
            Assert.NotEqual(flashcard2, flashcardSet.Flashcards[index]);
        }

        [Fact]
        public void DeleteFlashcard_PassingNonEmptyFlashcardSet_CorrectlyRemovesSpecifiedFlashcard()
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet();
            var flashcard1 = new Flashcard();
            var flashcard2 = new Flashcard();
            flashcardSet.Flashcards!.Add(flashcard1);
            flashcardSet.Flashcards.Add(flashcard2);

            // Act
            int index = 1;
            int result = service.DeleteFlashcard(index, flashcardSet);

            // Assert
            Assert.True(flashcardSet.Flashcards.Count == 1);
            Assert.Equal(1, result);
            Assert.Equal(flashcard1, flashcardSet.Flashcards[0]);
        }

        [Fact]
        public void SaveFlashcardSetName_PassingStringName_SetsNameCorrectly()
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet();
            string name = "Name";

            // Act 
            service.SaveFlashcardSetName(name, flashcardSet);
            string result = flashcardSet.FlashcardSetName;

            // Assert
            Assert.Equal(result, name);
        }

        [Theory]
        [InlineData (3, 2)]
        [InlineData (10, 9)]
        [InlineData (1, 0)]
        [InlineData (0, 0)]
        [InlineData (-1, 0)]
        public void CalculateSelectionIndexAfterDeletion_PassingIndexes_ReturnsCorrectIndex(int input, int expectedResult)
        {
            // Arrange
            // Act
            int result = service.CalculateSelectionIndexAfterDeletion(input);

            // Assert
            Assert.True(expectedResult == result);
        }

        [Theory]
        [InlineData ("word", "WORD")]
        [InlineData ("WORD", "WORD")]
        [InlineData ("", "")]
        public void CapitalizeFlashcardSetName_PassingWithCapitalizationNeeded_ReturnsCapitalizedString(string input, string expectedResult)
        {
            // Arrange
            bool isCapitalizationNeeded = true;
            
            // Act
            string result = service.CapitalizeFlashcardSetName(isCapitalizationNeeded, input, input);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("word", "word")]
        [InlineData("WORD", "WORD")]
        [InlineData("", "")]
        public void CapitalizeFlashcardSetName_PassingWithCapitalizationNotNeeded_ReturnsTheSameString(string input, string expectedResult)
        {
            // Arrange
            bool isCapitalizationNeeded = false;

            // Act
            string result = service.CapitalizeFlashcardSetName(isCapitalizationNeeded, input, input);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
