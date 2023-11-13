using FirstLab.src.interfaces;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace FirstLab.XAML;

public partial class LogsView : UserControl
{
    public ObservableCollection<FlashcardSetLog> flashcardSetsLogs = new ObservableCollection<FlashcardSetLog>();

    ILogsViewService _ifLogsViewService;

    public LogsView(ILogsViewService ifLogsViewService)
    {
        InitializeComponent();
        InitializeLogsFields(ifLogsViewService);
    }

    private async void InitializeLogsFields(ILogsViewService ifLogsViewService)
    {
        _ifLogsViewService = ifLogsViewService;
        await _ifLogsViewService.RetrieveLogs(flashcardSetsLogs);
        LogsItemsControl.ItemsSource = flashcardSetsLogs;
    }

    public async void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
    {
        await _ifLogsViewService.CreateLogAndSave(flashcardSet, playWindowStartTime, playWindowEndTime, flashcardSetsLogs);
    }

    private async void ClearLogs_Click(object sender, RoutedEventArgs e)
    {
        await _ifLogsViewService.ClearLogs(flashcardSetsLogs);
    }
}
