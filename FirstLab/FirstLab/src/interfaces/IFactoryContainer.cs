using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using FirstLab.src.errorHandling;
using FirstLab.src.models;
using FirstLab.src.exceptions;
using System.Windows;

namespace FirstLab.src.interfaces;

public interface IFactoryContainer
{
    T CreateWindow<T>(FlashcardSet? flashcardSet = null) where T : class;

    FlashcardSetLog CreateLog(string playedSetsName, DateTime date, int duration);

    FlashcardDesign CreateDesign(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize);

    CustomizationErrors CreateErrorHandling(TextBox errorTextBox, string? nameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards);

    T CreateObject<T>() where T : class, new();

    CustomNullException CreateException(string errorMsg);

    QuestionAnswerPropertiesForUI CreateQuestionAnswerProperties(Visibility QuestionBorderVisibility, Visibility AnswerBorderVisibility, bool CheckQuestionRadioButton, bool CheckAnswerRadioButton);
}
