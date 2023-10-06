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
        private MenuWindow menuWindowReference;

        private FlashcardCustomization flashcardCustomizationview;

        private PlayWindow playWindowReference;

        private ObservableCollection<FlashcardSet> flashcardSets;

        public DateTime playWindowStartTime;

        public FlashcardSet flashcardSet;

        public FlashcardOptions(MenuWindow menuWindowReference)
        {
            InitializeComponent();

            this.menuWindowReference = menuWindowReference;
            flashcardSets = this.menuWindowReference.flashcardSets;
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
            playWindowReference = new PlayWindow(menuWindowReference, this, (FlashcardSet)flashcardSetsControl.SelectedItem);
            ViewsUtils.ChangeWindow(menuWindowReference, "Play", playWindowReference: playWindowReference);
            playWindowStartTime = DateTime.Now;
            flashcardSet = (FlashcardSet)flashcardSetsControl.SelectedItem;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (flashcardSetsControl.SelectedItem is FlashcardSet selectedSet)
            {
                if (menuWindowReference != null)
                {
                    flashcardSets.Remove(selectedSet);
                }
            }

            flashcardSetsControl.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            flashcardCustomizationview = new FlashcardCustomization(menuWindowReference, this, (FlashcardSet)flashcardSetsControl.SelectedItem);
            ViewsUtils.ChangeWindow(menuWindowReference, "Customization", flashcardCustomizationView: flashcardCustomizationview);
        }

        private void NewSet_Click(object sender, RoutedEventArgs e)
        {
            flashcardCustomizationview = new FlashcardCustomization(menuWindowReference, this);
            ViewsUtils.ChangeWindow(menuWindowReference, "Customization", flashcardCustomizationView: flashcardCustomizationview);
        }
    }
}
