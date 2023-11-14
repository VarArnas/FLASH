using FirstLab.src.models.DTOs;
using FirstLab.src.models;
using FirstLab.src.interfaces;

namespace FirstLab.src.mappers;

public class FlashcardSetLogMapper : IFlashcardSetLogMapper
{
    private IFactoryContainer? _factoryContainer;

    public FlashcardSetLogMapper(IFactoryContainer factoryContainer)
    {
        _factoryContainer = factoryContainer;
    }

    public FlashcardSetLog TransformDTOtoFlashcardSetLog(FlashcardSetLogDTO dto)
    {
        FlashcardSetLog log = _factoryContainer!.CreateLog(dto.PlayedSetsName, dto.Date, dto.Duration);
        return log;
    }

    public FlashcardSetLogDTO TransformFlashcardSetLogtoDTO(FlashcardSetLog log)
    {
        FlashcardSetLogDTO dto = _factoryContainer!.CreateObject<FlashcardSetLogDTO>();
        dto.PlayedSetsName = log.PlayedSetsName;
        dto.Date = log.Date;
        dto.Duration = log.Duration;
        return dto;
    }
}
