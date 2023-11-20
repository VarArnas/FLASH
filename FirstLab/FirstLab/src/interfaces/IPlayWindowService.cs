using FirstLab.src.exceptions;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace FirstLab.src.interfaces;

public interface IPlayWindowService
{
    int FindCounter(int ind, FlashcardSet flashcardSet);

    void ShuffleFlashcards(ObservableCollection<Flashcard> flashcards);

    FlashcardSet CloneFlashcardSet(FlashcardSet originalSet);

    void ThrowCustomException(string message, Exception exception);

    void LogCustomException(string message);

    bool IsIndexOverBounds(int index, FlashcardSet flashcardSet);

    void HandleNullColor(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex);

    void HandleNullTimer(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex);

    DoubleAnimation SetAnimation();

    TextAndBorderPropertiesPlayWindow SetQuestionOrAnswerProperties(bool question, bool answer, int currentFlashcardIndex, FlashcardSet flashcardSet);

    TextModificationProperties SetTextProperties(bool isHighlighted, bool isItalic);

    double FindNewTextSize(bool increaseSize, FlashcardDesign flashcardDesign, double presentFontSize);

    string SetSlidePanelAnimation();

    void TryToIncrementCurrentIndex(ref int currentIndex, FlashcardSet flashcardSet);

    void CreateCounter(ref int counter, int ind, FlashcardSet flashcardSet);

    TextAndBorderPropertiesPlayWindow GetQuestionAnswerProperties(bool question, bool answer, int currentFlashcardIndex, FlashcardSet flashcardSet);

}