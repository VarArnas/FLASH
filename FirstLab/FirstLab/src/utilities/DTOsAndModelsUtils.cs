using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using System.Collections.ObjectModel;
using System.Linq;

namespace FirstLab.src.utilities;

public static class DTOsAndModelsUtils
{
    public static IFactoryContainer factoryContainer;

    public static FlashcardSetDTO TransformFlashcardSetToDTO(FlashcardSet flashcardSet)
    {
        FlashcardSetDTO set = factoryContainer.CreateObject<FlashcardSetDTO>();

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

    public static FlashcardSet TransformDTOtoFlashcardSet(FlashcardSetDTO dto)
    {
        FlashcardSet set = factoryContainer.CreateObject<FlashcardSet>();

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

    public static FlashcardSetLog TransformDTOtoFlashcardSetLog(FlashcardSetLogDTO dto)
    {
        FlashcardSetLog log = factoryContainer.CreateLog(dto.PlayedSetsName, dto.Date, dto.Duration);
        return log;
    }

    public static FlashcardSetLogDTO TransformFlashcardSetLogtoDTO(FlashcardSetLog log)
    {
        FlashcardSetLogDTO dto = factoryContainer.CreateObject<FlashcardSetLogDTO>();
        dto.PlayedSetsName = log.PlayedSetsName;
        dto.Date = log.Date;
        dto.Duration = log.Duration;
        return dto;
    }
}
