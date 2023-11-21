using FirstLab.src.exceptions;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace FirstLab.src.interfaces;

public interface IPlayWindowService
{
    int FindCounter(Flashcard flashcard);

    void ShuffleFlashcards(ObservableCollection<Flashcard> flashcards);

    FlashcardSet CloneFlashcardSet(FlashcardSet originalSet);

    void ThrowCustomException(string message, Exception exception);

    void LogCustomException(string message);

    void HandleNullColor(CustomNullException ex, Flashcard flashcard);

    void HandleNullTimer(CustomNullException ex);

    DoubleAnimation SetAnimation();

    TextAndBorderPropertiesPlayWindow SetQuestionOrAnswerProperties(bool question, bool answer, Flashcard flashcard, FlashcardSet flashcardSet);

    TextModificationProperties SetTextProperties(bool isHighlighted, bool isItalic);

    double FindNewTextSize(bool increaseSize, FlashcardDesign flashcardDesign, double presentFontSize);

    string SetSlidePanelAnimation();

    void CreateCounter(ref int counter, Flashcard flashcard);

    TextAndBorderPropertiesPlayWindow GetQuestionAnswerProperties(bool question, bool answer, Flashcard flashcard, FlashcardSet flashcardSet);

    bool IsLastIndex(int index, FlashcardSet flashcardSet);

    bool IsFirstOrZeroIndex(int index);

    int CheckIfPreviousOrNext(bool isPreviousFlashcardNeeded, int index, FlashcardSet flashcardSet, bool isStart);

}