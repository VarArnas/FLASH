using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Markup;
using FirstLab.src.models;
using FirstLab.src.utilities;

namespace FirstLab.src.interfaces;

public interface IFlashcardOptionsService
{
    Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets);

    void InitializeDatabase(IServiceProvider serviceProvider);

    Task InitializeFlashcardSets(ObservableCollection<FlashcardSet> flashcardSets);

    ObservableCollection<FlashcardSet> CalculateFlashcardSetDifficulties(ObservableCollection<FlashcardSet> flashcardSets);

    string CalculateDifficultyOfFlashcardSet(FlashcardSet flashcardSet);

    void GoToFlashcardCustomization(FlashcardSet? flashcardSet = null);

    void LaunchPlayWindow(FlashcardSet flashcardSet);
}