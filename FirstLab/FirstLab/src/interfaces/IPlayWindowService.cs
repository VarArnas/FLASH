using FirstLab.src.exceptions;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

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

    bool DetermineQuestionOrAnswer(bool question, bool answer);

    int GetFlashcardIndex(Flashcard flashcard, FlashcardSet flashcardSet);

    TextAndBorderPropertiesPlayWindow CreateTextAndBorderProperties(string flashcardNumber, string text, SolidColorBrush borderColor, Visibility questionVisibility, Visibility answerVisibility);

    int ParseCounter(string selectedTime);

    string CreateQuery(Flashcard currentFlashcard, string userAnswer);

    Task<string> CallOpenAIController(string query);

    double ExtractNumber(string result);

    SolidColorBrush? GetAnswerColor(double result);

}