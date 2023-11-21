using FirstLab.src.factories;
using FirstLab.src.mappers;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class FlashcardSetLogMapperTest
    {
        FlashcardSetLogMapper flashcardSetLogMapper;

        public FlashcardSetLogMapperTest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var factoryContainer = new FactoryContainer(serviceProviderMock.Object);
            flashcardSetLogMapper = new FlashcardSetLogMapper(factoryContainer);
        }

        [Fact]
        public void TransformDTOtoFlashcardSetLog_PassingAllStandartValues_ReturnsLogWithSameValues()
        {
            // Arrange
            FlashcardSetLogDTO flashcardSetLogDTO = new FlashcardSetLogDTO
            {
                PlayedSetsName = "a",
                Duration = 5,
                Date = new DateTime(2023, 11, 11)
            };

            // Act
            FlashcardSetLog flashcardSetLog = flashcardSetLogMapper.TransformDTOtoFlashcardSetLog(flashcardSetLogDTO);

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
            FlashcardSetLogDTO flashcardSetLogDTO = flashcardSetLogMapper.TransformFlashcardSetLogtoDTO(flashcardSetLog);

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
