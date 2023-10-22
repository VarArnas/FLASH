using FirstLab.src.back_end;
using FirstLab.src.back_end.data;
using FirstLab.src.back_end.factories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace FirstLab.XAML;

public partial class LogsView : UserControl
{
    public ObservableCollection<FlashcardSetLog> flashcardSetsLogs;

    private TimeSpan duration;

    private IMainFactories logsFactory;

    public LogsView(IMainFactories logsFactory)
    {
        InitializeComponent();
        InitializeLogsFields(logsFactory);
    }

    private async void InitializeLogsFields(IMainFactories logsFactory)
    {
        flashcardSetsLogs = await DatabaseRepository.GetAllAsync<FlashcardSetLog>();
        LogsItemsControl.ItemsSource = flashcardSetsLogs;
        this.logsFactory = logsFactory;
    }

    public async void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
    {
        duration = playWindowEndTime - playWindowStartTime;
        var log =  logsFactory.CreateLog(flashcardSet.FlashcardSetName, playWindowStartTime, (int)duration.TotalSeconds);
        flashcardSetsLogs.Insert(0, log);
        await DatabaseRepository.AddAsync(log);
    }

    private async void ClearLogs_Click(object sender, RoutedEventArgs e)
    {
        flashcardSetsLogs.Clear();
        await DatabaseRepository.RemoveAllAsync<FlashcardSetLog>();
    }
}
