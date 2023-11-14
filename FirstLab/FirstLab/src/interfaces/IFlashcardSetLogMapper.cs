using FirstLab.src.models;
using FirstLab.src.models.DTOs;

namespace FirstLab.src.interfaces
{
    public interface IFlashcardSetLogMapper
    {
        FlashcardSetLog TransformDTOtoFlashcardSetLog(FlashcardSetLogDTO dto);
        FlashcardSetLogDTO TransformFlashcardSetLogtoDTO(FlashcardSetLog log);
    }
}