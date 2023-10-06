﻿using FirstLab.src.back_end;
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
            flashcardSetsLogs = DataManager.LoadLogs();
            LogsItemsControl.ItemsSource = flashcardSetsLogs;
        }

        public void CalculateAndCreateLog(DateTime playWindowStartTime, DateTime playWindowEndTime, FlashcardSet flashcardSet)
        {
            duration = playWindowEndTime - playWindowStartTime;
            var log = new FlashcardSetLog(flashcardSet.FlashcardSetName, playWindowStartTime, (int)duration.TotalSeconds);
            flashcardSetsLogs.Insert(0, log);
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            flashcardSetsLogs.Clear();
        }
    }
}