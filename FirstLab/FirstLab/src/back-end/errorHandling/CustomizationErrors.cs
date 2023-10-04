using FirstLab.src.back_end.utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace FirstLab.src.back_end.errorHandling
{
    class CustomizationErrors
    {
        private TextBox errorTextBox;

        private string? NameOfFlashcardSet;

        private FlashcardSet flashcardSet;

        private ObservableCollection<FlashcardSet> SetsOfFlashcards;

        public enum ErrorCode
        {
            NoError,
            NameIsEmpty,
            NotAllowedSymbolsInName,
            ExistingName,
            NoFlashcardsExist,
            NotAllFlashcardsFull
        }

        public List<ErrorCode> ErrorCodes { get; private set; }

        public CustomizationErrors(TextBox errorTextBox, string? NameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards) 
        {
            this.errorTextBox = errorTextBox;
            this.NameOfFlashcardSet = NameOfFlashcardSet;
            this.flashcardSet = flashcardSet;
            this.SetsOfFlashcards = SetsOfFlashcards;
            ErrorCodes = new List<ErrorCode>();
        }

        private void CheckForErrors()
        {
            ErrorCodes.Clear();

            if (string.IsNullOrWhiteSpace(NameOfFlashcardSet))
            {
                ErrorCodes.Add(ErrorCode.NameIsEmpty);
            }

            if (NameOfFlashcardSet.ContainsSymbols())
            {
                ErrorCodes.Add(ErrorCode.NotAllowedSymbolsInName);
            }

            if (SetsOfFlashcards.Contains(flashcardSet))
            {
                ErrorCodes.Add(ErrorCode.ExistingName);
            }

            if (!flashcardSet.Flashcards.Any())
            {
                ErrorCodes.Add(ErrorCode.NoFlashcardsExist);
            }

            if (ErrorUtils.AreThereEmptyFlashcards(flashcardSet.Flashcards))
            {
                ErrorCodes.Add(ErrorCode.NotAllFlashcardsFull);
            }
        }

        private void DisplayErrors()
        {
            errorTextBox.Clear();

            foreach (ErrorCode errorCode in ErrorCodes)
            {
                switch (errorCode)
                {
                    case ErrorCode.NameIsEmpty:
                        errorTextBox.AppendText("Error: Name is empty!\n\n");
                        break;
                    case ErrorCode.NotAllowedSymbolsInName:
                        errorTextBox.AppendText("Error: Name contains symbols!\n\n");
                        break;
                    case ErrorCode.ExistingName:
                        errorTextBox.AppendText("Error: Name already exists!\n\n");
                        break;
                    case ErrorCode.NoFlashcardsExist:
                        errorTextBox.AppendText("Error: No flashcards exist!\n\n");
                        break;
                    case ErrorCode.NotAllFlashcardsFull:
                        errorTextBox.AppendText("Error: Not all flashcards have questions and answers!\n\n");
                        break;
                }
            }
        }

        public void CheckAndDisplayErrors()
        {
            CheckForErrors();
            DisplayErrors();
        }
    }
}
