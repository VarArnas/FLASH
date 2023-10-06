using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab.XAML
{
    public partial class PlayWindow : UserControl
    {
        private FlashcardSet flashcardSet;

        private int currentFlashcardIndex = 0;

        public PlayWindow(MenuWindow menuWindowReference, FlashcardOptions flashcardOptionsReference, FlashcardSet flashcardSet)
        {
            InitializeComponent();
            this.flashcardSet = CloneFlashcardSet(flashcardSet);

            Shuffle(this.flashcardSet.Flashcards);

            DataContext = this.flashcardSet;
            nameTextBox.Text = flashcardSet.FlashcardSetName;

            if (ListBoxFlashcards.Items.Count > 0)
            {
                DisplayFlashcard(currentFlashcardIndex);
            }
        }

        private void Shuffle(ObservableCollection<Flashcard> flashcards)
        {
            Random random = new Random();

            for (int i = flashcards.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                Flashcard temp = flashcards[i];
                flashcards[i] = flashcards[j];
                flashcards[j] = temp;
            }
        }

        private FlashcardSet CloneFlashcardSet(FlashcardSet originalSet)
        {
            FlashcardSet clonedSet = new FlashcardSet();
            foreach (var flashcard in originalSet.Flashcards)
            {
                clonedSet.Flashcards.Add(new Flashcard
                {
                    FlashcardQuestion = flashcard.FlashcardQuestion,
                    FlashcardAnswer = flashcard.FlashcardAnswer
                });
            }
            clonedSet.FlashcardSetName = originalSet.FlashcardSetName;
            return clonedSet;
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

        private void DisplayFlashcard(object sender, RoutedEventArgs e)
        {
            DisplayFlashcard(currentFlashcardIndex);
            currentFlashcardIndex++;
        }

        private void displayAnswer(object sender, RoutedEventArgs e)
        {
            DisplayAnswer(currentFlashcardIndex - 1);
        }
    }

}