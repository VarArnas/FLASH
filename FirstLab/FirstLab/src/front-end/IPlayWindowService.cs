using FirstLab.src.back_end.errorHandling;
using FirstLab.src.back_end.factories.factoryInterfaces;
using System;
using System.Collections.ObjectModel;

namespace FirstLab.src;

public interface IPlayWindowService
{
    int SetTheCounter(int ind, FlashcardSet flashcardSet);

    void ShuffleFlashcards(ObservableCollection<Flashcard> flashcards);

    FlashcardSet CloneFlashcardSet(FlashcardSet originalSet);

    void ThrowCustomException(string message, Exception exception);

    void LogCustomException(string message);

    bool IsIndexOverBounds(int index, FlashcardSet flashcardSet);

    void HandleNullColor(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex);

    void HandleNullTimer(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex);

}