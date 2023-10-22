

using FirstLab.src.back_end.errorHandling;
using FirstLab.XAML;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FirstLab.src.back_end.factories;

public class MainFactories : IMainFactories
{
    private readonly IServiceProvider serviceProvider;

    public MainFactories(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public PlayWindow CreatePlayWindow(FlashcardSet flashcardSet)
    {
        return ActivatorUtilities.CreateInstance<PlayWindow>(serviceProvider, flashcardSet);
    }

    public FlashcardCustomization CreateCustomizationView(FlashcardOptions flashcardOptions, FlashcardSet? flashcardSet = null)
    {
        if (flashcardSet == null)
        {
            return ActivatorUtilities.CreateInstance<FlashcardCustomization>(serviceProvider, flashcardOptions);
        }
        return ActivatorUtilities.CreateInstance<FlashcardCustomization>(serviceProvider, flashcardOptions, flashcardSet);
    }

    public FlashcardSetLog CreateLog(string playedSetsName, DateTime date, int duration)
    {
        return ActivatorUtilities.CreateInstance<FlashcardSetLog>(serviceProvider, playedSetsName, date, duration);
    }

    public FlashcardDesign CreateDesign(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize)
    {
        return ActivatorUtilities.CreateInstance<FlashcardDesign>(serviceProvider, isItalic, isHighlighted, increaseTextSize, decreaseTextSize);
    }

    public CustomizationErrors CreateErrorHandling(TextBox errorTextBox, string? NameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards)
    {
        return ActivatorUtilities.CreateInstance<CustomizationErrors>(serviceProvider, errorTextBox, NameOfFlashcardSet, flashcardSet, SetsOfFlashcards);
    }

    public Flashcard CreateFlashcard()
    {
        return ActivatorUtilities.CreateInstance<Flashcard>(serviceProvider);
    }

    public FlashcardSet CreateFlashcardSet()
    {
        return ActivatorUtilities.CreateInstance<FlashcardSet>(serviceProvider);
    }

    public ObservableCollection<T> CreateCollection<T>(List<T> entities)
    {
        return ActivatorUtilities.CreateInstance<ObservableCollection<T>>(serviceProvider, entities);
    }

    public string CreateString(Array arr)
    {
        return ActivatorUtilities.CreateInstance<string>(serviceProvider, arr);
    }
}
