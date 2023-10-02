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

        public FlashcardSet flashcardSet;

        public Flashcard flashcard;

        public PlayWindow(MenuWindow menuWindowReference, FlashcardSet flashcardSet)
        {
            InitializeComponent();

            this.menuWindowReference = menuWindowReference;

            this.flashcardSet = flashcardSet;
        }

        private void displayFlashcard(object sender, RoutedEventArgs e)
        {
                questionTextBox.Text = flashcardSet.Flashcards[1].FlashcardQuestion;
                answerTextBox.Text = flashcardSet.Flashcards[1].FlashcardAnswer;          
        }
    }
}
