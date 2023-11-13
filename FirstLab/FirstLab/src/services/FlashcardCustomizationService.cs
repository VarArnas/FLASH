using System.Threading.Tasks;
using FirstLab.src.interfaces;
using FirstLab.src.utilities;
using FirstLab.src.data;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;

namespace FirstLab.src.services;

public class FlashcardCustomizationService : IFlashcardCustomizationService
{
    IFactoryContainer _factoryContainer;

    IFlashcardSetMapper _ifFlashcardSetMapper;

    public FlashcardCustomizationService(IFactoryContainer factoryContainer, IFlashcardSetMapper ifFlashcardSetMapper)
    {
        _factoryContainer = factoryContainer;
        _ifFlashcardSetMapper = ifFlashcardSetMapper;
    }

    public async Task RemoveSetFromDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference)
    {
        flashcardOptionsReference.flashcardSets.Remove(flashcardSet);
        await DatabaseRepository.RemoveFlashcardSetAsync(flashcardSet.FlashcardSetName);
    }

    public int AddFlashcard(FlashcardSet flashcardSet)
    {
        var flashcard = _factoryContainer.CreateObject<Flashcard>();
        int flashcardNumber = flashcardSet.Flashcards!.Count + 1;
        flashcard.FlashcardName = flashcardNumber.ToString("D2");
        flashcardSet.Flashcards.Add(flashcard);
        return flashcardSet.Flashcards.IndexOf(flashcard);
    }

    public int DeleteFlashcard(int index, FlashcardSet flashcardSet)
    {
        if (flashcardSet.Flashcards!.Count > 1)
        {
            flashcardSet.Flashcards!.Remove(flashcardSet.Flashcards[index]);
            for (int i = index; i < flashcardSet.Flashcards.Count; i++)
            {
                flashcardSet.Flashcards[i].FlashcardName = (i + 1).ToString("D2");
            }
            return index;
        }
        return index;
    }

    public void SaveFlashcardSetName(string name, FlashcardSet flashcardSet)
    {
        flashcardSet.FlashcardSetName = name;
    }

    public async Task SaveToDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference)
    {
        flashcardOptionsReference.flashcardSets.Add(flashcardSet);

        FlashcardSetDTO dto = _ifFlashcardSetMapper.TransformFlashcardSetToDTO(flashcardSet);
        await DatabaseRepository.AddAsync(dto);
    }
}
