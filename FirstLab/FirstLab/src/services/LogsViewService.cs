using FirstLab.src.data;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.services;

public class LogsViewService : ILogsViewService
{
    IFlashcardSetLogMapper _flashcardSetLogMapper;

    IFactoryContainer _factoryContainer;

    public LogsViewService(IFlashcardSetLogMapper ifFLashcardSetLogMapper, IFactoryContainer factoryContainer)
    {
        _flashcardSetLogMapper = ifFLashcardSetLogMapper;
        _factoryContainer = factoryContainer;
    }

    public async Task AddLog(FlashcardSetLog log, ObservableCollection<FlashcardSetLog>? flashcardSetsLogs)
    {
        FlashcardSetLogDTO dto = _flashcardSetLogMapper.TransformFlashcardSetLogtoDTO(log);
        flashcardSetsLogs!.Insert(0, log);
        await DatabaseRepository.AddAsync(dto);
    }

    public async Task RetrieveLogs(ObservableCollection<FlashcardSetLog> logs)
    {
        ObservableCollection<FlashcardSetLogDTO> dtos = await DatabaseRepository.GetAllAsync<FlashcardSetLogDTO>();

        foreach (var dto in dtos)
        {
            FlashcardSetLog log = _flashcardSetLogMapper.TransformDTOtoFlashcardSetLog(dto);
            logs.Add(log);
        }
    }

    public async Task ClearLogs(ObservableCollection<FlashcardSetLog> logs)
    {
        logs.Clear();
        await DatabaseRepository.RemoveAllAsync<FlashcardSetLogDTO>();
    }

    public async Task CreateLogAndSave(FlashcardSet flashcardSet, DateTime playWindowStartTime, DateTime playWindowEndTime,
        ObservableCollection<FlashcardSetLog> flashcardSetsLogs)
    {
        TimeSpan duration = CalculateLog(playWindowStartTime, playWindowEndTime);
        var log = _factoryContainer.CreateLog(flashcardSet.FlashcardSetName, playWindowStartTime, (int)duration.TotalSeconds);
        await AddLog(log, flashcardSetsLogs);
    }

    public TimeSpan CalculateLog(DateTime playWindowStartTime, DateTime playWindowEndTime)
    {
        return playWindowEndTime - playWindowStartTime;
    }
}
