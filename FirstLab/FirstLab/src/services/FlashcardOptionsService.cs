using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using FirstLab.XAML;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.services;

public class FlashcardOptionsService : IFlashcardOptionsService
{
    IFactoryContainer _factoryContainer;

    IFlashcardSetMapper _flashcardSetMapper;

    IDatabaseRepository _databaseRepository;

    public FlashcardOptionsService(IFactoryContainer factoryContainer, IFlashcardSetMapper flashcardSetMapper, IDatabaseRepository databaseRepository)
    {
        _factoryContainer = factoryContainer;
        _flashcardSetMapper = flashcardSetMapper;
        _databaseRepository = databaseRepository;
    }

    public async Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets)
    {
        await _databaseRepository.RemoveFlashcardSetAsync(selectedSet.FlashcardSetName);
        flashcardSets.Remove(selectedSet);
    }

    public async Task<ObservableCollection<FlashcardSet>> InitializeFlashcardSets()
    {
        ObservableCollection<FlashcardSetDTO> flashcardSetsDTOs = await _databaseRepository.GetAllFlashcardSetsAsync();
        var flashcardSets = new ObservableCollection<FlashcardSet>();

        foreach(var dto in flashcardSetsDTOs)
        {
            FlashcardSet set = _flashcardSetMapper.TransformDTOtoFlashcardSet(dto);
            flashcardSets!.Add(set);
        }

        return flashcardSets;
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
