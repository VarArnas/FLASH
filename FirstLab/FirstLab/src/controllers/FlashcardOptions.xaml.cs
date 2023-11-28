using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.services;
using FirstLab.src.utilities;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab.src.controllers;

public partial class FlashcardOptions : UserControl
{
    public ObservableCollection<FlashcardSet> flashcardSets;

    public ObservableCollection<FlashcardSet> FlashcardSets
    {
        get
        {
            flashcardSets.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedEventHandler);
            return flashcardSets;
        }
        set
        {
            flashcardSets = value;
        }
    }

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
        FlashcardSets = _flashcardOptionsService.CalculateFlashcardSetDifficulties(FlashcardSets);
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

    private void CollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            FlashcardSet flashcardSet = e.NewItems.Cast<FlashcardSet>().First();
            if (flashcardSet != null)
            {
                flashcardSet.FlashcardSetDifficulty = _flashcardOptionsService.CalculateDifficultyOfFlashcardSet(flashcardSet);
            }
        }
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
