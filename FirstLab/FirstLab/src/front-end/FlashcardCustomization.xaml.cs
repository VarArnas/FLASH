using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using FirstLab.src.back_end.errorHandling;
using FirstLab.src.back_end.utilities;

namespace FirstLab
{
    public partial class FlashcardCustomization : UserControl
    {
        private FlashcardSet flashcardSet;

        private MenuWindow menuWindowReference;

        private FlashcardOptions flashcardOptionsReference;

        private string? NameOfSet;

        private CustomizationErrors errors;
        public FlashcardCustomization(MenuWindow menuWindowReference, FlashcardOptions flashcardOptionsReference, FlashcardSet? flashcardSet = null)
        {
            InitializeComponent();

            this.menuWindowReference = menuWindowReference;
            this.flashcardOptionsReference = flashcardOptionsReference;
            this.flashcardSet = flashcardSet ?? new FlashcardSet();
            DataContext = this.flashcardSet;

            if (flashcardSet == null)
            {
                QuestionTextBox.IsEnabled = false;
                AnswerTextBox.IsEnabled = false;
                QuestionBorder.Visibility = Visibility.Collapsed;
                AnswerBorder.Visibility = Visibility.Collapsed;
                QuestionRadioButton.Visibility = Visibility.Collapsed;
                AnswerRadioButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                menuWindowReference.flashcardSets.Remove(flashcardSet);
                ListBoxFlashcards.SelectedIndex = flashcardSet.Flashcards.Count - 1;
                NameOfSet = flashcardSet.FlashcardSetName;
            }
        }

        private void AddFlashcard_Click(object sender, RoutedEventArgs e)
        {
            var newFlashcard = new Flashcard();
            int newFlashcardNumber = flashcardSet.Flashcards.Count + 1;
            newFlashcard.FlashcardName = "#" + newFlashcardNumber.ToString();
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
            if (flashcardSet.Flashcards.Count > 0)
            {
                int selectedIndex = ListBoxFlashcards.SelectedIndex;
                flashcardSet.Flashcards.Remove((Flashcard)ListBoxFlashcards.SelectedItem);
                ListBoxFlashcards.Items.Refresh();
                ListBoxFlashcards.SelectedIndex = (selectedIndex - 1 < 0) ? 0 : selectedIndex - 1;

                for (int i = selectedIndex; i < flashcardSet.Flashcards.Count; i++)
                {
                    flashcardSet.Flashcards[i].FlashcardName = "#" + (i + 1);
                }
            }
        }

        private void QuestionAnswerRadioButton_Click(object sender, RoutedEventArgs e)
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
            if (QuestionRadioButton.IsChecked == true)
            {
                QuestionTextBox.Focus();
            }
            else 
            {
                AnswerTextBox.Focus();
            }
        }

        private void CapitalizedNormalNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (CapitalizeButton.IsChecked == true)
            {
                NameOfSet = FlashcardSetNameBox.Text;
                FlashcardSetNameBox.Text = NameOfSet.Capitalize();
                flashcardSet.FlashcardSetName = NameOfSet.Capitalize();
            }
            else
            {
                FlashcardSetNameBox.Text = NameOfSet;
                flashcardSet.FlashcardSetName = NameOfSet;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            flashcardSet.FlashcardSetName = flashcardSet.FlashcardSetName;
            CapitalizeButton.IsChecked = false;
            NormalizeButton.IsChecked = true;
            CapitalizedNormalNameButton_Click(NormalizeButton, new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NameOfSet = FlashcardSetNameBox.Text;
        }

        private void DeleteFlashcardSet_Click(object sender, RoutedEventArgs e)
        {
            ViewsUtils.ChangeWindow(menuWindowReference, "Flashcards", flashcardOptionsReference);
        }

        private void SaveFlashcardSet_Click(object sender, RoutedEventArgs e)
        {
            errors = new CustomizationErrors(flashcardSet: flashcardSet, NameOfFlashcardSet: FlashcardSetNameBox.Text, errorTextBox: errorText, SetsOfFlashcards: menuWindowReference.flashcardSets);
            errors.CheckAndDisplayErrors();
            if (!errors.ErrorCodes.Any())
            {
                menuWindowReference.flashcardSets.Add(flashcardSet);
                ViewsUtils.ChangeWindow(menuWindowReference, "Flashcards", flashcardOptionsReference);     
            }
        }

        private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorBox.SelectedItem != null)
            {
                ListBoxItem selectedColorItem = (ListBoxItem)ColorBox.SelectedItem;

                string flashcardColorT = selectedColorItem.ToString();
                int indexOfColon = flashcardColorT.IndexOf(":");

                if (indexOfColon != -1)
                {
                    flashcardColorT = flashcardColorT.Substring(indexOfColon + 2);
                }

                if (!string.IsNullOrEmpty(selectedColorItem.ToString()))
                {
                    SolidColorBrush colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcardColorT);

                    QuestionBorder.Background = colorBrush;
                    AnswerBorder.Background = colorBrush;
                }
            }
        }
    }
}
