using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.interfaces
{
    public interface ILogsViewService
    {
        Task AddLog(FlashcardSetLog log, ObservableCollection<FlashcardSetLog>? flashcardSetsLogs);

        Task<ObservableCollection<FlashcardSetLog>> RetrieveLogs();

        Task ClearLogs(ObservableCollection<FlashcardSetLog> logs);

        TimeSpan CalculateLog(DateTime playWindowStartTime, DateTime playWindowEndTime);

        Task CreateLogAndSave(FlashcardSet flashcardSet, DateTime playWindowStartTime, DateTime playWindowEndTime,
        ObservableCollection<FlashcardSetLog> flashcardSetsLogs);
    }
}