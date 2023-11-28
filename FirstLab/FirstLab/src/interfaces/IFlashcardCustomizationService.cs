using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using FirstLab.src.controllers;
using FirstLab.src.errorHandling;
using FirstLab.src.models;

namespace FirstLab.src.interfaces;

public interface IFlashcardCustomizationService
{
    int AddFlashcard(FlashcardSet flashcardSet);

    int DeleteFlashcard(int index, FlashcardSet flashcardSet);

    Task RemoveSetFromDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference);

    void SaveFlashcardSetName(string name, FlashcardSet flashcardSet);

    Task SaveToDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference);

    QuestionAnswerPropertiesForUI ChangeQuestionAnswerProperties(bool question, bool answer);

    int CalculateSelectionIndexAfterDeletion(int oldIndex);

    string CapitalizeFlashcardSetName(bool? isCapitalizationNeeded, string NameOfSet, string flashcardSetNameTextBox);

    CustomizationErrors InitializeErrors(FlashcardSet flashcardSet, string nameOfFlashcardSet, TextBox errorText, ObservableCollection<FlashcardSet> SetsOfFlashcards);

    bool IsFlashcardSetCorrect(FlashcardSet flashcardSet, string nameOfFlashcardSet, TextBox errorText, ObservableCollection<FlashcardSet> SetsOfFlashcards);

    int CanYouChangeFlashcards(int currentIndex, FlashcardSet flashcardSet, int direction);

    Task CheckErrorsAndSaveFlashcard(FlashcardSet flashcardSet, string nameOfFlashcardSet, TextBox errorText, ObservableCollection<FlashcardSet> SetsOfFlashcards,
    FlashcardOptions flashcardOptions);

    bool CheckIfEditingAndRemoveTheOldFlashcardSet(FlashcardSet? flashcardSet, FlashcardOptions flashcardOptionsReference, string? NameOfSet);
}