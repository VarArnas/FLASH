using System;
using System.Collections.Generic;
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
    /// Interaction logic for FlashCardCustomization.xaml
    /// </summary>
    public partial class FlashcardCustomization : UserControl
    {
        private FlashcardSet flashcardSet = new FlashcardSet();

        private MenuWindow menuWindowReference;
        public FlashcardCustomization(MenuWindow menuWindowReference, FlashcardSet optionalFlashcardSet = null)
        {
            InitializeComponent();

            this.menuWindowReference = menuWindowReference;

            if (optionalFlashcardSet != null)
            {
                DataContext = optionalFlashcardSet;
            }
            else
            {
                DataContext = flashcardSet; 
            }

            
            QuestionTextBox.IsEnabled = false;
            AnswerTextBox.IsEnabled = false;
            QuestionBorder.Visibility = Visibility.Collapsed;
            AnswerBorder.Visibility = Visibility.Collapsed;

            QuestionRadioButton.Visibility = Visibility.Collapsed;
            AnswerRadioButton.Visibility = Visibility.Collapsed;
        }

        private void AddFlashcard_Click(object sender, RoutedEventArgs e)
        {

            var newFlashcard = new Flashcard();
            int newFlashcardNumber = flashcardSet.Flashcards.Count + 1;
            newFlashcard.CardName = "#" + newFlashcardNumber.ToString();
            flashcardSet.Flashcards.Add(newFlashcard);
            ListBoxFlashcards.Items.Refresh();
            ListBoxFlashcards.SelectedIndex = flashcardSet.Flashcards.IndexOf(newFlashcard);

            QuestionBorder.Visibility = Visibility.Visible;
            QuestionRadioButton.Visibility = Visibility.Visible;
            QuestionRadioButton.IsChecked = true;
            QuestionTextBox.Visibility = Visibility.Visible;
            QuestionTextBox.IsEnabled = true;
            QuestionTextBox.Focus();
            AnswerBorder.Visibility = Visibility.Collapsed;

            AnswerRadioButton.Visibility = Visibility.Visible;

        }

        private void ListBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxFlashcards.SelectedItem != null)
            {
                QuestionBorder.Visibility = Visibility.Visible;
                AnswerBorder.Visibility = Visibility.Collapsed;
                QuestionRadioButton.IsChecked = true;
                QuestionTextBox.IsEnabled = true;
            }
        }

        private void DeleteFlashcard_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBoxFlashcards.SelectedIndex;
            flashcardSet.Flashcards.Remove((Flashcard)ListBoxFlashcards.SelectedItem);
            ListBoxFlashcards.Items.Refresh();

            for (int i = selectedIndex; i < flashcardSet.Flashcards.Count; i++)
            {
                flashcardSet.Flashcards[i].CardName = "#" + (i + 1);
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionRadioButton.IsChecked == true)
            {
                QuestionBorder.Visibility = Visibility.Visible;
                AnswerBorder.Visibility = Visibility.Collapsed;
                QuestionTextBox.IsEnabled = true;
                AnswerTextBox.IsEnabled = false;
            }
            else
            {
                QuestionBorder.Visibility = Visibility.Collapsed;
                AnswerBorder.Visibility = Visibility.Visible;
                QuestionTextBox.IsEnabled = false;
                AnswerTextBox.IsEnabled = true;
            }
        }

        private void QuestionBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // focus the TextBox when the user clicks anywhere inside the border
            if(QuestionRadioButton.IsChecked == true)
            {
                QuestionTextBox.Focus();
            }
            else 
            {
                AnswerTextBox.Focus();
            }
        }
    }
}
