using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstLab
{
    /// <summary>
    /// Interaction logic for FlashcardOptions.xaml
    /// </summary>
    public partial class FlashcardOptions : UserControl
    {
        private MenuWindow menuWindowReference;

        private FlashcardCustomization flashcardCustomizationview;

        private ObservableCollection<FlashcardSet> flashcardSets;

        public FlashcardOptions(ObservableCollection<FlashcardSet> flashcardSets, MenuWindow menuWindowReference)
        {
            InitializeComponent();

            flashcardSetsControl.ItemsSource = flashcardSets; //initialize the Itemcontrol with flashcardsets Observable Collection

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
            // need to create a new window to play the flashcards
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
            // add error window
            // make it so u can delete by hovering instead of clicking on the set
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            flashcardCustomizationview = new FlashcardCustomization(menuWindowReference);
            menuWindowReference.UpdateHeaderText("Customization");
            menuWindowReference.contentControl.Content = flashcardCustomizationview;
            // add exit button
        }


        private void NewSet_Click(object sender, RoutedEventArgs e)
        {
            flashcardCustomizationview = new FlashcardCustomization(menuWindowReference);
            menuWindowReference.UpdateHeaderText("Customization");
            //For Now Gotta change later
            menuWindowReference.contentControl.Content = flashcardCustomizationview;
        }
    }
}
