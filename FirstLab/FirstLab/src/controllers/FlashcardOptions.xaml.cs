using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab;

public partial class FlashcardOptions : UserControl
{
    public ObservableCollection<FlashcardSet> flashcardSets = new();

    public DateTime playWindowStartTime;

    public FlashcardSet flashcardSet;

    IFlashcardOptionsService _ifFlashcardOptionsService;

    public FlashcardOptions(IServiceProvider serviceProvider, IFlashcardOptionsService ifFlashcardOptionsService)
    {
        InitializeComponent();
        ifFlashcardOptionsService.InitializeDatabase(serviceProvider);
        InitializeOptionsFields(ifFlashcardOptionsService);
    }

    private async void InitializeOptionsFields(IFlashcardOptionsService ifFlashcardOptionsService)
    {
        _ifFlashcardOptionsService = ifFlashcardOptionsService;
        await _ifFlashcardOptionsService.InitializeFlashcardSets(flashcardSets);
        flashcardSetsControl.ItemsSource = flashcardSets;
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        TextUtils.SetEmptyText(searchBox, "search...");
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        TextUtils.SetDefaultText(searchBox, "search...");
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        _ifFlashcardOptionsService.LaunchPlayWindow((FlashcardSet)flashcardSetsControl.SelectedItem);
        InitializeTimeForLog();
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        await _ifFlashcardOptionsService.RemoveFlashcardSet((FlashcardSet)flashcardSetsControl.SelectedItem, flashcardSets);
        flashcardSetsControl.Items.Refresh();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        _ifFlashcardOptionsService.GoToFlashcardCustomization((FlashcardSet)flashcardSetsControl.SelectedItem);
    }

    private void NewSet_Click(object sender, RoutedEventArgs e)
    {
        _ifFlashcardOptionsService.GoToFlashcardCustomization();
    }

    private void InitializeTimeForLog()
    {
        playWindowStartTime = DateTime.Now;
        flashcardSet = (FlashcardSet)flashcardSetsControl.SelectedItem;
    }
}
