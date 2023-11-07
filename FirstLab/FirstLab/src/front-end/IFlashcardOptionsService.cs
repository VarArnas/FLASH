using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.front_end;

public interface IFlashcardOptionsService
{
    Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets);

    void InitializeDatabase(IServiceProvider serviceProvider);

    Task InitializeFlashcardSets(ObservableCollection<FlashcardSet> flashcardSets);
}