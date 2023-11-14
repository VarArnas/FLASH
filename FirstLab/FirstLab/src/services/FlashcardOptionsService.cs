using FirstLab.src.data;
using FirstLab.src.factories;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using FirstLab.XAML;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.services;

public class FlashcardOptionsService : IFlashcardOptionsService
{
    IFactoryContainer _factoryContainer;

    IFlashcardSetMapper _ifFlashcardSetMapper;

    public FlashcardOptionsService(IFactoryContainer factoryContainer, IFlashcardSetMapper ifFlashcardSetMapper)
    {
        _factoryContainer = factoryContainer;
        _ifFlashcardSetMapper = ifFlashcardSetMapper;
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
            FlashcardSet set = _ifFlashcardSetMapper.TransformDTOtoFlashcardSet(dto);
            flashcardSets!.Add(set);
        }
    }

    public void GoToFlashcardCustomization(FlashcardSet? flashcardSet = null)
    {
        FlashcardCustomization flashcardCustomization = _factoryContainer.CreateWindow<FlashcardCustomization>(flashcardSet);
        ViewsUtils.ChangeWindow("Customization", flashcardCustomization);
    }

    public void LaunchPlayWindow(FlashcardSet flashcardSet)
    {
        PlayWindow playWindowReference = _factoryContainer.CreateWindow<PlayWindow>(flashcardSet);
        ViewsUtils.menuWindowReference!.Hide();
        playWindowReference.Show();
    }
}
