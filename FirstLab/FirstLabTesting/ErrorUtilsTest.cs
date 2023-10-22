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
        [InlineData("question", "answer", false)]
        [InlineData("", "answer", true)]
        [InlineData("  ", "answer", true)]
        [InlineData(null, "answer", true)]
        [InlineData("question", "", true)]
        [InlineData("question", "  ", true)]
        [InlineData("question", null, true)]
        public void ErrorUtils_IsFlashcardEmpty_ReturnsBool(string question, string answer, bool expected)
        {
            // Arrange
            Flashcard flashcard = new Flashcard { FlashcardQuestion = question, FlashcardAnswer = answer};
            
            // Act
            var result = ErrorUtils.IsFlashcardEmpty(flashcard);

            // Assert  
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ErrorUtils_AreThereEmptyFlashcards_ReturnsBool()
        {
            // Arrange
            ObservableCollection<Flashcard> flashcardSet1 = new ObservableCollection<Flashcard> { 
                new Flashcard{ FlashcardQuestion = "question1", FlashcardAnswer = "answer1"},
                new Flashcard{ FlashcardQuestion = "question2", FlashcardAnswer = "answer2"} 
            };
            ObservableCollection<Flashcard> flashcardSet2 = new ObservableCollection<Flashcard>();
            ObservableCollection<Flashcard> flashcardSet3 = new ObservableCollection<Flashcard> {
                new Flashcard{ FlashcardQuestion = "", FlashcardAnswer = "answer1"},
                new Flashcard{ FlashcardQuestion = "question2", FlashcardAnswer = "answer2"}
            };

            // Act 
            var result1 = ErrorUtils.AreThereEmptyFlashcards(flashcardSet1);
            var result2 = ErrorUtils.AreThereEmptyFlashcards(flashcardSet2);
            var result3 = ErrorUtils.AreThereEmptyFlashcards(flashcardSet3);

            // Assert
            Assert.False(result1);
            Assert.False(result2);
            Assert.True(result3);
        }
    }
}
