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

    IDatabaseRepository _databaseRepository;

    public LogsViewService(IFlashcardSetLogMapper fLashcardSetLogMapper, IFactoryContainer factoryContainer, IDatabaseRepository databaseRepository)
    {
        _flashcardSetLogMapper = fLashcardSetLogMapper;
        _factoryContainer = factoryContainer;
        _databaseRepository = databaseRepository;
    }

    public async Task AddLog(FlashcardSetLog log, ObservableCollection<FlashcardSetLog>? flashcardSetsLogs)
    {
        FlashcardSetLogDTO dto = _flashcardSetLogMapper.TransformFlashcardSetLogtoDTO(log);
        flashcardSetsLogs!.Insert(0, log);
        await _databaseRepository.AddAsync(dto);
    }

    public async Task<ObservableCollection<FlashcardSetLog>> RetrieveLogs()
    {
        ObservableCollection<FlashcardSetLogDTO> dtos = await _databaseRepository.GetAllAsync<FlashcardSetLogDTO>();
        var logs = new ObservableCollection<FlashcardSetLog>();

        foreach (var dto in dtos)
        {
            FlashcardSetLog log = _flashcardSetLogMapper.TransformDTOtoFlashcardSetLog(dto);
            logs.Add(log);
        }

        return logs;
    }

    public async Task ClearLogs(ObservableCollection<FlashcardSetLog> logs)
    {
        logs.Clear();
        await _databaseRepository.RemoveAllAsync<FlashcardSetLogDTO>();
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
