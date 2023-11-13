using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.utilities;
using FirstLab.XAML;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab;

public partial class FlashcardOptions : UserControl
{
    private FlashcardCustomization flashcardCustomizationview;

    private PlayWindow playWindowReference;

    public ObservableCollection<FlashcardSet> flashcardSets = new ObservableCollection<FlashcardSet>();

    public DateTime playWindowStartTime;

    public FlashcardSet flashcardSet;

    IFactoryContainer factoryContainer;

    IFlashcardOptionsService _controllerService;

    public FlashcardOptions(IFactoryContainer factoryContainer, IServiceProvider serviceProvider, IFlashcardOptionsService controllerService)
    {
        InitializeComponent();
        controllerService.InitializeDatabase(serviceProvider);
        controllerService.InitializeUtilities();
        InitializeOptionsFields(factoryContainer, controllerService);
    }

    private async void InitializeOptionsFields(IFactoryContainer factoryContainer, IFlashcardOptionsService controllerService)
    {
        _controllerService = controllerService;
        await _controllerService.InitializeFlashcardSets(flashcardSets);
        flashcardSetsControl.ItemsSource = flashcardSets;
        this.factoryContainer = factoryContainer;
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
        playWindowReference = factoryContainer.CreateWindow<PlayWindow>((FlashcardSet)flashcardSetsControl.SelectedItem);
        ViewsUtils.menuWindowReference!.Hide();
        playWindowReference.Show();
        InitializeTimeForLog();
    }

    private void InitializeTimeForLog()
    {
        playWindowStartTime = DateTime.Now;
        flashcardSet = (FlashcardSet)flashcardSetsControl.SelectedItem;
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        await _controllerService.RemoveFlashcardSet((FlashcardSet)flashcardSetsControl.SelectedItem, flashcardSets);
        flashcardSetsControl.Items.Refresh();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        flashcardCustomizationview = factoryContainer.CreateWindow<FlashcardCustomization>((FlashcardSet)flashcardSetsControl.SelectedItem);
        ViewsUtils.ChangeWindow("Customization", flashcardCustomizationview);
    }

    private void NewSet_Click(object sender, RoutedEventArgs e)
    {
        flashcardCustomizationview = factoryContainer.CreateWindow<FlashcardCustomization>();
        ViewsUtils.ChangeWindow("Customization", flashcardCustomizationview);
    }
}
