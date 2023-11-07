using FirstLab.src.data;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.controllers.services;

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
        ObservableCollection<FlashcardSetDTO> flashcardSetsDTOs = await DatabaseRepository.GetAllFlashcardSetsAsync();

        foreach(var dto in flashcardSetsDTOs)
        {
            FlashcardSet set = DTOsAndModelsUtils.TransformDTOtoFlashcardSet(dto);
            flashcardSets!.Add(set);
        }
    }

    public void InitializeUtilities()
    {
        DTOsAndModelsUtils.factoryContainer = _factoryContainer;
    }
}
