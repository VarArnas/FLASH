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
using System.Windows.Shapes;

namespace FirstLab.XAML
{
    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    /// 
    public partial class PlayWindow : UserControl
    {
        private MenuWindow menuWindowReference;

        private FlashcardSet flashcardSet;

        private FlashcardOptions flashcardOptionsReference;

        private int currentFlashcardIndex = 0; 

        public PlayWindow(MenuWindow menuWindowReference, FlashcardOptions flashcardOptionsReference, FlashcardSet flashcardSet)
        {
            InitializeComponent();

            this.menuWindowReference = menuWindowReference;
            this.flashcardOptionsReference = flashcardOptionsReference;
            this.flashcardSet = flashcardSet;

            DataContext = this.flashcardSet;
            nameTextBox.Text = flashcardSet.FlashcardSetName;

            if (ListBoxFlashcards.Items.Count > 0)
            {
                DisplayFlashcard(currentFlashcardIndex);
            }
        }


        private void DisplayFlashcard(int index)
        {
            if (index >= 0 && index < ListBoxFlashcards.Items.Count)
            {
                questionTextBox.Text = flashcardSet.Flashcards[index].FlashcardQuestion;
                answerTextBox.Clear();
            }
        }

        private void DisplayAnswer(int index)
        {
            if (index >= 0 && index < ListBoxFlashcards.Items.Count)
            {
                answerTextBox.Text = flashcardSet.Flashcards[index].FlashcardAnswer;
            }
        }

        private void displayFlashcard(object sender, RoutedEventArgs e)
        {
            DisplayFlashcard(currentFlashcardIndex);
            currentFlashcardIndex++;
        }

        private void displayAnswer(object sender, RoutedEventArgs e)
        {
            DisplayAnswer(currentFlashcardIndex-1);
        }
    }

}
