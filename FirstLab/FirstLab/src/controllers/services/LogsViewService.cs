using FirstLab.src.data;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.controllers.services;

public class LogsViewService : ILogsViewService
{
    IFactoryContainer _factoryContainer;

    public LogsViewService(IFactoryContainer factoryContainer)
    {
        _factoryContainer = factoryContainer;
    }

    public async Task AddLog(FlashcardSetLog log, ObservableCollection<FlashcardSetLog>? flashcardSetsLogs)
    {
        FlashcardSetLogDTO dto = DTOsAndModelsUtils.TransformFlashcardSetLogtoDTO(log);
        flashcardSetsLogs.Insert(0, log);
        await DatabaseRepository.AddAsync(dto);
    }

    public async Task RetrieveLogs(ObservableCollection<FlashcardSetLog> logs)
    {
        ObservableCollection<FlashcardSetLogDTO> dtos = await DatabaseRepository.GetAllAsync<FlashcardSetLogDTO>();

        foreach (var dto in dtos)
        {
            FlashcardSetLog log = DTOsAndModelsUtils.TransformDTOtoFlashcardSetLog(dto);
            logs.Add(log);
        }
    }

    public async Task ClearLogs(ObservableCollection<FlashcardSetLog> logs)
    {
        logs.Clear();
        await DatabaseRepository.RemoveAllAsync<FlashcardSetLogDTO>();
    }
}
