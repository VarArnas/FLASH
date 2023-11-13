using FirstLab.src.interfaces;
using FirstLab.src.models.DTOs;
using FirstLab.src.models;
using System.Collections.ObjectModel;
using System.Linq;

namespace FirstLab.src.mappers;

public class FlashcardSetMapper : IFlashcardSetMapper
{
    IFactoryContainer _factoryContainer;

    public FlashcardSetMapper(IFactoryContainer factoryContainer)
    {
        _factoryContainer = factoryContainer;
    }
    public FlashcardSetDTO TransformFlashcardSetToDTO(FlashcardSet flashcardSet)
    {
        FlashcardSetDTO set = _factoryContainer!.CreateObject<FlashcardSetDTO>();

        set.FlashcardSetName = flashcardSet.FlashcardSetName;

        set.Flashcards = new ObservableCollection<FlashcardDTO>(
            flashcardSet.Flashcards!.Select(flashcardSet => new FlashcardDTO
            {
                FlashcardName = flashcardSet.FlashcardName,
                FlashcardQuestion = flashcardSet.FlashcardQuestion,
                FlashcardAnswer = flashcardSet.FlashcardAnswer,
                FlashcardColor = flashcardSet.FlashcardColor,
                FlashcardTimer = flashcardSet.FlashcardTimer
            }));

        return set;
    }

    public FlashcardSet TransformDTOtoFlashcardSet(FlashcardSetDTO dto)
    {
        FlashcardSet set = _factoryContainer!.CreateObject<FlashcardSet>();

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

        return set;
    }
}
