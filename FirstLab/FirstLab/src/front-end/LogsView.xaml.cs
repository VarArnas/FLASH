using FirstLab.src.back_end;
using FirstLab.src.back_end.data;
using FirstLab.src.back_end.factories.factoryInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace FirstLab.XAML;

public partial class LogsView : UserControl
{
    public ObservableCollection<FlashcardSetLogDTO> _flashcardSetsLogs;

    private TimeSpan duration;

    IFactoryContainer factoryContainer;

    public LogsView(IFactoryContainer factoryContainer)
    {
        InitializeComponent();
        InitializeLogsFields(factoryContainer);
    }

    private async void InitializeLogsFields(IFactoryContainer factoryContainer)
    {
        this.factoryContainer = factoryContainer; 
        _flashcardSetsLogs = await DatabaseRepository.GetAllAsync<FlashcardSetLogDTO>();
        LogsItemsControl.ItemsSource = _flashcardSetsLogs;
    }

    public async void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
    {
        duration = playWindowEndTime - playWindowStartTime;
        var log = factoryContainer.CreateLog(flashcardSet.FlashcardSetName, playWindowStartTime, (int)duration.TotalSeconds);
        FlashcardSetLogDTO temp = new FlashcardSetLogDTO();
        temp.Duration = log.Duration;
        temp.Date = log.Date;
        temp.PlayedSetsName = log.PlayedSetsName;
        _flashcardSetsLogs.Insert(0, temp);
        await DatabaseRepository.AddAsync(temp);
    }

    private async void ClearLogs_Click(object sender, RoutedEventArgs e)
    {
        _flashcardSetsLogs.Clear();
        await DatabaseRepository.RemoveAllAsync<FlashcardSetLog>();
    }
}
