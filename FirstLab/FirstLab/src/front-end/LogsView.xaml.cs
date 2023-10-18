using FirstLab.src.back_end;
using FirstLab.src.back_end.data;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace FirstLab.XAML
{
    public partial class LogsView : UserControl
    {
        public ObservableCollection<FlashcardSetLog> flashcardSetsLogs;

        private TimeSpan duration;

        public LogsView()
        {
            InitializeComponent();
            InitializeLogsFields();
        }

        private async void InitializeLogsFields()
        {
            flashcardSetsLogs = await DatabaseRepository.GetAllAsync<FlashcardSetLog>();
            LogsItemsControl.ItemsSource = flashcardSetsLogs;
        }

        public async void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
        {
            duration = playWindowEndTime - playWindowStartTime;
            var log = new FlashcardSetLog(flashcardSet.FlashcardSetName, playWindowStartTime, (int)duration.TotalSeconds);
            flashcardSetsLogs.Insert(0, log);
            await DatabaseRepository.AddAsync(log);
        }

        private async void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            flashcardSetsLogs.Clear();
            await DatabaseRepository.RemoveAllAsync<FlashcardSetLog>();
        }
    }
}
