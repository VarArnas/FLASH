﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab
{
    public partial class FlashcardOptions : UserControl
    {
        private MenuWindow menuWindowReference;

        private FlashcardCustomization flashcardCustomizationview;

        private ObservableCollection<FlashcardSet> flashcardSets;

        public FlashcardOptions(ObservableCollection<FlashcardSet> flashcardSets, MenuWindow menuWindowReference)
        {
            InitializeComponent();

            flashcardSetsControl.ItemsSource = flashcardSets;

            this.menuWindowReference = menuWindowReference;

            this.flashcardSets = flashcardSets;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "search...")
            {
                searchBox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            searchBox.Text = "search...";
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {

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
            if (flashcardSetsControl.SelectedItem is FlashcardSet selectedSet)
            {
                flashcardCustomizationview = new FlashcardCustomization(menuWindowReference, selectedSet);
                menuWindowReference.UpdateHeaderText("Customization");
                menuWindowReference.contentControl.Content = flashcardCustomizationview;
            }
        }


        private void NewSet_Click(object sender, RoutedEventArgs e)
        {
            flashcardCustomizationview = new FlashcardCustomization(menuWindowReference);
            menuWindowReference.UpdateHeaderText("Customization");
            menuWindowReference.contentControl.Content = flashcardCustomizationview;
        }
    }
}
