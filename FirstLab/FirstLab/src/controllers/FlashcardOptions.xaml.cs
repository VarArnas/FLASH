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

namespace FirstLab;

public partial class FlashcardOptions : UserControl
{
    public ObservableCollection<FlashcardSet> flashcardSets = new();

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
        await _ifFlashcardOptionsService.InitializeFlashcardSets(FlashcardSets);
        FlashcardSets = _ifFlashcardOptionsService.CalculateFlashcardSetDifficulties(FlashcardSets);
        flashcardSetsControl.ItemsSource = FlashcardSets;
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
                flashcardSet.FlashcardSetDifficulty = _ifFlashcardOptionsService.CalculateDifficultyOfFlashcardSet(flashcardSet);
            }
        }
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        _ifFlashcardOptionsService.LaunchPlayWindow((FlashcardSet)flashcardSetsControl.SelectedItem);
        InitializeTimeForLog();
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        await _ifFlashcardOptionsService.RemoveFlashcardSet((FlashcardSet)flashcardSetsControl.SelectedItem, FlashcardSets);
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
