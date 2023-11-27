using FirstLab.src.factories;
using FirstLab.src.mappers;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using Moq;

namespace FirstLabTesting
{
    public class FlashcardSetMapperTest
    {
        FlashcardSetMapper flashcardSetMapper;
        
        public FlashcardSetMapperTest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var factoryContainer = new FactoryContainer(serviceProviderMock.Object);
            flashcardSetMapper = new FlashcardSetMapper(factoryContainer);
        }

        [Fact]
        public void TransformFlashcardSetToDTO_PassingAllStandartValues_ReturnsDTOWithSameValues()
        {
            // Arrange
            FlashcardSet flashcardSet = new FlashcardSet { FlashcardSetName = "Test" };
            var mockedFlashcard = new Flashcard
            {
                FlashcardName = "a",
                FlashcardQuestion = "b",
                FlashcardAnswer = "c",
                FlashcardColor = "d",
                FlashcardTimer = "e"
            };

            flashcardSet.Flashcards.Add(mockedFlashcard);

            // Act
            FlashcardSetDTO flashcardSetDTO = flashcardSetMapper.TransformFlashcardSetToDTO(flashcardSet);

            // Assert
            Assert.NotNull(flashcardSetDTO);
            Assert.Equal(flashcardSet.FlashcardSetName, flashcardSetDTO.FlashcardSetName);
            Assert.NotEmpty(flashcardSetDTO.Flashcards);
            Assert.Collection(flashcardSetDTO.Flashcards,
                flashcard =>
                {
                    Assert.IsType<FlashcardDTO>(flashcard);
                    Assert.Equal(mockedFlashcard.FlashcardName, flashcard.FlashcardName);
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
            FlashcardSetDTO flashcardSetDTO = flashcardSetMapper.TransformFlashcardSetToDTO(flashcardSet);

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
            FlashcardSet flashcardSet = flashcardSetMapper.TransformDTOtoFlashcardSet(flashcardSetDTO);

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
            FlashcardSet flashcardSet = flashcardSetMapper.TransformDTOtoFlashcardSet(flashcardSetDTO);

            // Assert
            Assert.NotNull(flashcardSet);
            Assert.Null(flashcardSet.FlashcardSetName);
            Assert.Empty(flashcardSet.Flashcards);
        }
    }
}
