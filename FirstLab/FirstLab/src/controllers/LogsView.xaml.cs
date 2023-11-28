using FirstLab.src.interfaces;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace FirstLab.src.controllers;

public partial class LogsView : UserControl
{
    public ObservableCollection<FlashcardSetLog> flashcardSetsLogs;

    ILogsViewService _logsViewService;

    public LogsView(ILogsViewService ifLogsViewService)
    {
        InitializeComponent();
        InitializeLogsFields(ifLogsViewService);
    }

    private async void InitializeLogsFields(ILogsViewService logsViewService)
    {
        _logsViewService = logsViewService;
        flashcardSetsLogs = await _logsViewService.RetrieveLogs();
        LogsItemsControl.ItemsSource = flashcardSetsLogs;
    }

    public async void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
    {
        await _logsViewService.CreateLogAndSave(flashcardSet, playWindowStartTime, playWindowEndTime, flashcardSetsLogs);
    }

    private async void ClearLogs_Click(object sender, RoutedEventArgs e)
    {
        await _logsViewService.ClearLogs(flashcardSetsLogs);
    }
}
