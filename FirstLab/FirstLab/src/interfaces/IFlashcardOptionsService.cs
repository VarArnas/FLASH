using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FirstLab.src.models;

namespace FirstLab.src.interfaces;

public interface IFlashcardOptionsService
{
    Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets);

    void InitializeDatabase(IServiceProvider serviceProvider);

    Task InitializeFlashcardSets(ObservableCollection<FlashcardSet> flashcardSets);

    void InitializeUtilities();
}