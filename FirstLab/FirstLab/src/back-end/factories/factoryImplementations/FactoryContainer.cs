using FirstLab.src.back_end.errorHandling;
using FirstLab.src.back_end.factories.factoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FirstLab.src.back_end.factories.factoryImplementations;

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

    public CustomizationErrors CreateErrorHandling(TextBox errorTextBox, string? nameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards)
    {
        return ActivatorUtilities.CreateInstance<CustomizationErrors>(serviceProvider, errorTextBox, nameOfFlashcardSet!, flashcardSet, SetsOfFlashcards);
    }

    public T CreateObject<T>() where T : class, new()
    {
        return ActivatorUtilities.CreateInstance<T>(serviceProvider);
    }

    public CustomNullException CreateException(string errorMsg)
    {
        var constructor = typeof(CustomNullException).GetConstructor(new[] { typeof(string) });
        return (CustomNullException)constructor.Invoke(new object[] { errorMsg });
    }
}
