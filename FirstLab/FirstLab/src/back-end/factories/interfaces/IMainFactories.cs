using FirstLab.src.back_end.errorHandling;
using FirstLab.XAML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FirstLab.src.back_end.factories;

public interface IMainFactories
{
    PlayWindow CreatePlayWindow(FlashcardSet flashcardSet);

    FlashcardCustomization CreateCustomizationView(FlashcardOptions flashcardOptions, FlashcardSet? flashcardSet = null);

    FlashcardSetLog CreateLog(string playedSetsName, DateTime date, int duration);

    FlashcardDesign CreateDesign(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize);

    CustomizationErrors CreateErrorHandling(TextBox errorTextBox, string? NameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards);

    Flashcard CreateFlashcard();

    FlashcardSet CreateFlashcardSet();

    ObservableCollection<T> CreateCollection<T>(List<T> entities);

    string CreateString(Array arr);
}