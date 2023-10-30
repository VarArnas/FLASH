using FirstLab;
using FirstLab.src.back_end.errorHandling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class ErrorUtilsTest
    {
        [Theory]
        [InlineData("", "answer")]
        [InlineData("  ", "answer")]
        [InlineData("question", "")]
        [InlineData("question", "  ")]
        
        public void IsFlashcardEmpty_PassingAnEmptyString_ReturnsTrue(string question, string answer)
        {
           // Arrange
           Flashcard flashcard = new Flashcard { FlashcardQuestion = question, FlashcardAnswer = answer };

           // Act
           var result = ErrorUtils.IsFlashcardEmpty(flashcard);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(null, "answer")]
        [InlineData("question", null)]
        [InlineData(null, null)]
        public void IsFlashcardEmpty_HandlingNull_ReturnsTrue(string question, string answer)
        {
            // Arrange
            Flashcard flashcard = new Flashcard { FlashcardQuestion = question, FlashcardAnswer = answer };

            // Act
            var result = ErrorUtils.IsFlashcardEmpty(flashcard);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsFlashcardEmpty_PassingNonEmptyStrings_ReturnsFalse()
        {
            // Arrange
            Flashcard flashcard = new Flashcard { FlashcardQuestion = "question", FlashcardAnswer = "answer" };

            // Act
            var result = ErrorUtils.IsFlashcardEmpty(flashcard);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreThereEmptyFlashcards_PassingAnEmptyFlashcard_ReturnsTrue()
        {
            // Arrange
            ObservableCollection<Flashcard> flashcardSet = new ObservableCollection<Flashcard> {
                new Flashcard{ FlashcardQuestion = "", FlashcardAnswer = "answer1"},
                new Flashcard{ FlashcardQuestion = "question2", FlashcardAnswer = "answer2"}
            };

            // Act 
            var result = ErrorUtils.AreThereEmptyFlashcards(flashcardSet);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AreThereEmptyFlashcards_PassingOnlyNonEmptyFlashcards_ReturnsFalse()
        {
            // Arrange
            ObservableCollection<Flashcard> flashcardSet = new ObservableCollection<Flashcard> {
                new Flashcard{ FlashcardQuestion = "question1", FlashcardAnswer = "answer1"},
                new Flashcard{ FlashcardQuestion = "question2", FlashcardAnswer = "answer2"}
            };

            // Act
            var result = ErrorUtils.AreThereEmptyFlashcards(flashcardSet);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreThereEmptyFlashcards_PassingEmptyFlashcardSet_ReturnsFalse()
        {
            // Arrange
            ObservableCollection<Flashcard> flashcardSet = new ObservableCollection<Flashcard>();

            // Act
            var result = ErrorUtils.AreThereEmptyFlashcards(flashcardSet);

            // Assert
            Assert.False(result);
        }

    }
}
