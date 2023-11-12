using FirstLab.src.factories;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class DTOsAndModelsUtilsTest
    {
        public DTOsAndModelsUtilsTest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var factoryContainer = new FactoryContainer(serviceProviderMock.Object);
            DTOsAndModelsUtils.factoryContainer = factoryContainer;
        }

        [Fact]
        public void TransformFlashcardSetToDTO_PassingAllStandartValues_ReturnsDTOWithSameValues()
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet { FlashcardSetName = "Test"};
            flashcardSet.Flashcards.Add(new Flashcard {
                FlashcardName = "a",
                FlashcardQuestion = "b",
                FlashcardAnswer = "c",
                FlashcardColor = "d",
                FlashcardTimer = "e"
            });

            // Act
            FlashcardSetDTO flashcardSetDTO = DTOsAndModelsUtils.TransformFlashcardSetToDTO(flashcardSet);

            // Assert
            Assert.NotNull(flashcardSetDTO);
            Assert.Equal("Test", flashcardSetDTO.FlashcardSetName);
            Assert.NotEmpty(flashcardSetDTO.Flashcards);
            Assert.Collection(flashcardSetDTO.Flashcards,
                flashcard =>
                {
                    Assert.IsType<FlashcardDTO>(flashcard);
                    Assert.Equal("a", flashcard.FlashcardName);
                    Assert.Equal("b", flashcard.FlashcardQuestion);
                    Assert.Equal("c", flashcard.FlashcardAnswer);
                    Assert.Equal("d", flashcard.FlashcardColor);
                    Assert.Equal("e", flashcard.FlashcardTimer);
                }
            );
        }

        [Fact]
        public void TransformFlashcardSetToDTO_HandlesNull_ReturnsDTOWithSameValues()
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet();

            // Act
            FlashcardSetDTO flashcardSetDTO = DTOsAndModelsUtils.TransformFlashcardSetToDTO(flashcardSet);

            // Assert
            Assert.NotNull(flashcardSetDTO);
            Assert.Null(flashcardSetDTO.FlashcardSetName);
            Assert.Empty(flashcardSetDTO.Flashcards);
        }

        [Fact]
        public void TransformDTOtoFlashcardSet_PassingAllStandartValues_ReturnsSetWithSameValues()
        {
            // Arrange
            FlashcardSetDTO flashcardSetDTO = new FlashcardSetDTO { FlashcardSetName = "Test" };
            flashcardSetDTO.Flashcards.Add(new FlashcardDTO
            {
                FlashcardName = "a",
                FlashcardQuestion = "b",
                FlashcardAnswer = "c",
                FlashcardColor = "d",
                FlashcardTimer = "e"
            });

            // Act
            FlashcardSet flashcardSet = DTOsAndModelsUtils.TransformDTOtoFlashcardSet(flashcardSetDTO);

            // Assert
            Assert.NotNull(flashcardSet);
            Assert.Equal("Test", flashcardSet.FlashcardSetName);
            Assert.NotEmpty(flashcardSet.Flashcards);
            Assert.Collection(flashcardSet.Flashcards,
                flashcard =>
                {
                    Assert.IsType<Flashcard>(flashcard);
                    Assert.Equal("a", flashcard.FlashcardName);
                    Assert.Equal("b", flashcard.FlashcardQuestion);
                    Assert.Equal("c", flashcard.FlashcardAnswer);
                    Assert.Equal("d", flashcard.FlashcardColor);
                    Assert.Equal("e", flashcard.FlashcardTimer);
                }
            );
        }

        [Fact]
        public void TransformDTOtoFlashcardSet_HandlesNull_ReturnsSetWithSameValues()
        {
            // Arrange
            FlashcardSetDTO flashcardSetDTO = new FlashcardSetDTO();

            // Act
            FlashcardSet flashcardSet = DTOsAndModelsUtils.TransformDTOtoFlashcardSet(flashcardSetDTO);

            // Assert
            Assert.NotNull(flashcardSet);
            Assert.Null(flashcardSet.FlashcardSetName);
            Assert.Empty(flashcardSet.Flashcards);
        }

        [Fact]
        public void TransformDTOtoFlashcardSetLog_PassingAllStandartValues_ReturnsLogWithSameValues()
        {
            // Arrange
            FlashcardSetLogDTO flashcardSetLogDTO = new FlashcardSetLogDTO {
                PlayedSetsName = "a",
                Duration = 5,
                Date = new DateTime(2023, 11, 11)
            };

            // Act
            FlashcardSetLog flashcardSetLog = DTOsAndModelsUtils.TransformDTOtoFlashcardSetLog(flashcardSetLogDTO);

            // Assert
            Assert.NotNull(flashcardSetLog);
            Assert.Equal("a", flashcardSetLog.PlayedSetsName);
            Assert.True(flashcardSetLog.Duration == 5);
            Assert.True(flashcardSetLog.Date.Year == 2023);
            Assert.True(flashcardSetLog.Date.Month == 11);
            Assert.True(flashcardSetLog.Date.Day == 11);
        }

        [Fact]
        public void TransformFlashcardSetLogtoDTO_PassingAllStandartValues_ReturnsDTOWithSameValues()
        {
            // Arrange
            FlashcardSetLog flashcardSetLog = new FlashcardSetLog("a", new DateTime(2023, 11, 11), 5);

            // Act
            FlashcardSetLogDTO flashcardSetLogDTO = DTOsAndModelsUtils.TransformFlashcardSetLogtoDTO(flashcardSetLog);

            // Assert
            Assert.NotNull(flashcardSetLogDTO);
            Assert.Equal("a", flashcardSetLogDTO.PlayedSetsName);
            Assert.True(flashcardSetLogDTO.Duration == 5);
            Assert.True(flashcardSetLogDTO.Date.Year == 2023);
            Assert.True(flashcardSetLogDTO.Date.Month == 11);
            Assert.True(flashcardSetLogDTO.Date.Day == 11);
        }

    }
}
