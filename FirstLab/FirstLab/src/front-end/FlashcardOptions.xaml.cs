using FirstLab.src.back_end.data;
using FirstLab.src.back_end.factories.factoryInterfaces;
using FirstLab.src.back_end.utilities;
using FirstLab.XAML;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab
{
    public partial class FlashcardOptions : UserControl
    {

        private FlashcardCustomization flashcardCustomizationview;

        private PlayWindow playWindowReference;

        public ObservableCollection<FlashcardSet> flashcardSets;

        public DateTime playWindowStartTime;

        public FlashcardSet flashcardSet;

        IFactoryContainer factoryContainer;

        public FlashcardOptions(IFactoryContainer factoryContainer, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            InitializeDatabase(serviceProvider, factoryContainer);
            InitializeOptionsFields(factoryContainer);
        }

        private void InitializeDatabase(IServiceProvider serviceProvider, IFactoryContainer factoryContainer)
        {
            DatabaseRepository.serviceProvider = serviceProvider;
            DatabaseRepository.factoryContainer = factoryContainer;
        }

        private async void InitializeOptionsFields(IFactoryContainer factoryContainer)
        {
            flashcardSets = await DatabaseRepository.GetAllFlashcardSetsAsync();
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
            ViewsUtils.ChangeWindow("Play", playWindowReference);
            playWindowStartTime = DateTime.Now;
            flashcardSet = (FlashcardSet)flashcardSetsControl.SelectedItem;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (flashcardSetsControl.SelectedItem is FlashcardSet selectedSet)
            {
               await DatabaseRepository.RemoveFlashcardSetAsync(selectedSet);
                flashcardSets.Remove(selectedSet);
            }

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
}
