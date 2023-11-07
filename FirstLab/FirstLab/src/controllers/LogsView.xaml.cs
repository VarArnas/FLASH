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

    private TimeSpan duration;

    IFactoryContainer factoryContainer;

    ILogsViewService controllerService;

    public LogsView(IFactoryContainer factoryContainer, ILogsViewService controllerService)
    {
        InitializeComponent();
        InitializeLogsFields(factoryContainer, controllerService);
    }

    private async void InitializeLogsFields(IFactoryContainer factoryContainer, ILogsViewService controllerService)
    {
        this.controllerService = controllerService;
        this.factoryContainer = factoryContainer;
        await controllerService.RetrieveLogs(flashcardSetsLogs);
        LogsItemsControl.ItemsSource = flashcardSetsLogs;
    }

    public async void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
    {
        duration = playWindowEndTime - playWindowStartTime;
        var log = factoryContainer.CreateLog(flashcardSet.FlashcardSetName, playWindowStartTime, (int)duration.TotalSeconds);
        await controllerService.AddLog(log, flashcardSetsLogs);
    }

    private async void ClearLogs_Click(object sender, RoutedEventArgs e)
    {
        await controllerService.ClearLogs(flashcardSetsLogs);
    }
}
