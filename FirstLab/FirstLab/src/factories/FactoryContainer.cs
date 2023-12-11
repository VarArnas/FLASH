using FirstLab.src.errorHandling;
using FirstLab.src.exceptions;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FirstLab.src.factories;

public class FactoryContainer : IFactoryContainer
{
    private readonly IServiceProvider serviceProvider;

    public FactoryContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public T CreateWindow<T>(FlashcardSet? flashcardSet = null) where T : class
    {
        if (flashcardSet == null)
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider);
        }
        return ActivatorUtilities.CreateInstance<T>(serviceProvider, flashcardSet);
    }

    public FlashcardSetLog CreateLog(string playedSetsName, DateTime date, int duration)
    {
        return ActivatorUtilities.CreateInstance<FlashcardSetLog>(serviceProvider, playedSetsName, date, duration);
    }

    public FlashcardDesign CreateDesign(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize)
    {
        return ActivatorUtilities.CreateInstance<FlashcardDesign>(serviceProvider, isItalic, isHighlighted, increaseTextSize, decreaseTextSize);
    }

    public CustomizationErrors CreateErrorHandling(string? nameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards, TextBox? errorTextBox = null)
    {
        if(errorTextBox == null)
        {
            return ActivatorUtilities.CreateInstance<CustomizationErrors>(serviceProvider, nameOfFlashcardSet!, flashcardSet, SetsOfFlashcards);
        }
        return ActivatorUtilities.CreateInstance<CustomizationErrors>(serviceProvider, nameOfFlashcardSet!, flashcardSet, SetsOfFlashcards, errorTextBox);
    }

    public T CreateObject<T>() where T : class, new()
    {
        return ActivatorUtilities.CreateInstance<T>(serviceProvider);
    }

    public CustomNullException CreateException(string errorMsg)
    {
        var constructor = typeof(CustomNullException).GetConstructor(new[] { typeof(string) });
        return (CustomNullException)constructor!.Invoke(new object[] { errorMsg });
    }

    public QuestionAnswerPropertiesForUI CreateQuestionAnswerProperties(Visibility QuestionBorderVisibility, Visibility AnswerBorderVisibility, bool CheckQuestionRadioButton, bool CheckAnswerRadioButton)
    {
        return ActivatorUtilities.CreateInstance<QuestionAnswerPropertiesForUI>(serviceProvider, QuestionBorderVisibility, AnswerBorderVisibility, CheckQuestionRadioButton, CheckAnswerRadioButton);
    }

    public TextAndBorderPropertiesPlayWindow CreateTextAndBorderPropertiesPlayWindow(string FlashcardNumberText, string QuestionText,
        SolidColorBrush? BorderColor, Visibility QuestionBorderVisibility, Visibility AnswerBorderVisibility)
    {
        return ActivatorUtilities.CreateInstance<TextAndBorderPropertiesPlayWindow>(serviceProvider, FlashcardNumberText, QuestionText, BorderColor, QuestionBorderVisibility!, AnswerBorderVisibility!);
    }

    public TextModificationProperties CreateTextModificationProperties(bool HighlightBtn, bool ItalicBtn, FontWeight QuestionAnswerTextWeight, FontStyle QuestionAnswerTextStyle)
    {
        return ActivatorUtilities.CreateInstance<TextModificationProperties>(serviceProvider, HighlightBtn, ItalicBtn, QuestionAnswerTextWeight, QuestionAnswerTextStyle);
    }
}
