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

        private enum ErrorCode
        {
            NoError = 0,
            NameIsEmpty = 1,
            NotAllowedSymbolsInName = 2,
            ExistingName = 3,
            NoFlashcardsExist = 4,
            NotAllFlashcardsFull = 5
        }

        private List<ErrorCode> errorCodes;

        public CustomizationErrors(TextBox errorTextBox, string? NameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards) 
        {
            this.errorTextBox = errorTextBox;
            this.NameOfFlashcardSet = NameOfFlashcardSet;
            this.flashcardSet = flashcardSet;
            this.SetsOfFlashcards = SetsOfFlashcards;
            errorCodes = new List<ErrorCode>();
        }

        private void CheckForErrors()
        {
            errorCodes.Clear();

            if (string.IsNullOrWhiteSpace(NameOfFlashcardSet))
            {
                errorCodes.Add(ErrorCode.NameIsEmpty);
            }

            if (NameOfFlashcardSet.ContainsSymbols())
            {
                errorCodes.Add(ErrorCode.NotAllowedSymbolsInName);
            }

            if (ErrorUtils.NameExists(NameOfFlashcardSet, SetsOfFlashcards))
            {
                errorCodes.Add(ErrorCode.ExistingName);
            }

            if (!flashcardSet.Flashcards.Any())
            {
                errorCodes.Add(ErrorCode.NoFlashcardsExist);
            }

            if (ErrorUtils.AreThereEmptyFlashcards(flashcardSet.Flashcards))
            {
                errorCodes.Add(ErrorCode.NotAllFlashcardsFull);
            }
        }

        private void DisplayErrors()
        {
            errorTextBox.Clear();

            foreach (ErrorCode errorCode in errorCodes)
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
