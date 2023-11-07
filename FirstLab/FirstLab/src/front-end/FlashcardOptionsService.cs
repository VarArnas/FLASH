using FirstLab.src.back_end;
using FirstLab.src.back_end.data;
using FirstLab.src.back_end.factories.factoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FirstLab.src.front_end;

public class FlashcardOptionsService : IFlashcardOptionsService
{
    IFactoryContainer _factoryContainer;
    public FlashcardOptionsService(IFactoryContainer factoryContainer)
    {
        _factoryContainer = factoryContainer;
    }

    public async Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets)
    {
        await DatabaseRepository.RemoveFlashcardSetAsync(selectedSet.FlashcardSetName);
        flashcardSets.Remove(selectedSet);
    }

    public void InitializeDatabase(IServiceProvider serviceProvider)
    {
        DatabaseRepository.serviceProvider = serviceProvider;
    }

    public async Task InitializeFlashcardSets(ObservableCollection<FlashcardSet>? flashcardSets)
    {
        ObservableCollection<FlashcardSetDTO> _flashcardSets = await DatabaseRepository.GetAllFlashcardSetsAsync();

        foreach(var dto in _flashcardSets)
        {
            FlashcardSet set = _factoryContainer.CreateObject<FlashcardSet>();

            set.FlashcardSetName = dto.FlashcardSetName;

            set.Flashcards = new ObservableCollection<Flashcard>(
                dto.Flashcards!.Select(dto => new Flashcard
                {
                    FlashcardName = dto.FlashcardName,
                    FlashcardQuestion = dto.FlashcardQuestion,
                    FlashcardAnswer = dto.FlashcardAnswer,
                    FlashcardColor = dto.FlashcardColor,
                    FlashcardTimer = dto.FlashcardTimer
                }));
            flashcardSets.Add(set);
        }
    }
}
