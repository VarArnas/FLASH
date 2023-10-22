using FirstLab.src.back_end.data;
using FirstLab.src.back_end.factories;
using FirstLab.src.back_end.utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab;

public partial class FlashcardOptions : UserControl
{
    private IMainFactories factories;

    public ObservableCollection<FlashcardSet> flashcardSets;

    public DateTime playWindowStartTime;

    public FlashcardSet flashcardSet;

    public FlashcardOptions(IMainFactories factories, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        InitializeDatabase(serviceProvider, factories);
        InitializeOptionsFields(factories);
    }

    private async void InitializeDatabase(IServiceProvider serviceProvider, IMainFactories factories)
    {
        DatabaseRepository.serviceProvider = serviceProvider; 
        DatabaseRepository.mainFactories = factories;
    }
    
    private async void InitializeOptionsFields(IMainFactories factories)
    {
        flashcardSets = await DatabaseRepository.GetAllFlashcardSetsAsync();
        flashcardSetsControl.ItemsSource = flashcardSets;
        this.factories = factories;
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
        var playView = factories.CreatePlayWindow((FlashcardSet)flashcardSetsControl.SelectedItem);
        ViewsUtils.ChangeWindow("Play", playView);
        playWindowStartTime = DateTime.Now;
        flashcardSet = (FlashcardSet)flashcardSetsControl.SelectedItem;
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (flashcardSetsControl.SelectedItem is FlashcardSet selectedSet)
        {
           await DatabaseRepository.RemoveAsync(selectedSet);
           flashcardSets.Remove(selectedSet);
        }
        flashcardSetsControl.Items.Refresh();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var customizationView = factories.CreateCustomizationView(this, (FlashcardSet)flashcardSetsControl.SelectedItem);
        ViewsUtils.ChangeWindow("Customization", customizationView);
    }

    private void NewSet_Click(object sender, RoutedEventArgs e)
    {
        var customizationView = factories.CreateCustomizationView(this);
        ViewsUtils.ChangeWindow("Customization", customizationView);
    }
}
