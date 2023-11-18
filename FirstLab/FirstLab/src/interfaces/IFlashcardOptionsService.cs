using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FirstLab.src.models;

namespace FirstLab.src.interfaces;

public interface IFlashcardOptionsService
{
    Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets);

    Task<ObservableCollection<FlashcardSet>> InitializeFlashcardSets();

    void GoToFlashcardCustomization(FlashcardSet? flashcardSet = null);

    void LaunchPlayWindow(FlashcardSet flashcardSet);
}