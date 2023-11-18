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
    public ObservableCollection<FlashcardSet> flashcardSets;

    public DateTime playWindowStartTime;

    public FlashcardSet flashcardSet;

    IFlashcardOptionsService _flashcardOptionsService;

    public FlashcardOptions(IFlashcardOptionsService flashcardOptionsService)
    {
        InitializeComponent();
        InitializeOptionsFields(flashcardOptionsService);
    }

    private async void InitializeOptionsFields(IFlashcardOptionsService flashcardOptionsService)
    {
        _flashcardOptionsService = flashcardOptionsService;
        flashcardSets = await _flashcardOptionsService.InitializeFlashcardSets();
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
        _flashcardOptionsService.LaunchPlayWindow((FlashcardSet)flashcardSetsControl.SelectedItem);
        InitializeTimeForLog();
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        await _flashcardOptionsService.RemoveFlashcardSet((FlashcardSet)flashcardSetsControl.SelectedItem, flashcardSets);
        flashcardSetsControl.Items.Refresh();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        _flashcardOptionsService.GoToFlashcardCustomization((FlashcardSet)flashcardSetsControl.SelectedItem);
    }

    private void NewSet_Click(object sender, RoutedEventArgs e)
    {
        _flashcardOptionsService.GoToFlashcardCustomization();
    }

    private void InitializeTimeForLog()
    {
        playWindowStartTime = DateTime.Now;
        flashcardSet = (FlashcardSet)flashcardSetsControl.SelectedItem;
    }
}
