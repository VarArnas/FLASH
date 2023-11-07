using FirstLab.src.models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.interfaces
{
    public interface ILogsViewService
    {
        Task AddLog(FlashcardSetLog log, ObservableCollection<FlashcardSetLog>? flashcardSetsLogs);

        Task RetrieveLogs(ObservableCollection<FlashcardSetLog> logs);

        Task ClearLogs(ObservableCollection<FlashcardSetLog> logs);
    }
}