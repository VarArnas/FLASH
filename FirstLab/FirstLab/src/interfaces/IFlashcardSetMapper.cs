using FirstLab.src.models;
using FirstLab.src.models.DTOs;

namespace FirstLab.src.interfaces
{
    public interface IFlashcardSetMapper
    {
        FlashcardSet TransformDTOtoFlashcardSet(FlashcardSetDTO dto);
        FlashcardSetDTO TransformFlashcardSetToDTO(FlashcardSet flashcardSet);
    }
}