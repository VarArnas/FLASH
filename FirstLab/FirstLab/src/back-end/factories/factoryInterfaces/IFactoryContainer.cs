

using FirstLab.src.back_end.errorHandling;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using System.Threading;

namespace FirstLab.src.back_end.factories.factoryInterfaces;

public interface IFactoryContainer
{
    T CreateWindow<T>(FlashcardSet? flashcardSet = null) where T : class;

    FlashcardSetLog CreateLog(string playedSetsName, DateTime date, int duration);

    FlashcardDesign CreateDesign(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize);

    CustomizationErrors CreateErrorHandling(TextBox errorTextBox, string? nameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards);

    T CreateObject<T>() where T : class;

    ObservableCollection<T> CreateCollection<T>(List<T>? entities = null);

    string CreateString(Array arr);

    Thread CreateThread(ThreadStart start);
}
