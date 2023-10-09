<<<<<<< Updated upstream
﻿using System;
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
=======
﻿using FirstLab.src.back_end;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
>>>>>>> Stashed changes

namespace FirstLab.XAML
{
    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
<<<<<<< Updated upstream
        public PlayWindow()
        {
            InitializeComponent();
        }
    }
}
=======
        private FlashcardSet flashcardSet;

        private FlashcardDesign flashcardDesign;

        private int currentFlashcardIndex = 0;

        private int incrementTextSize = 5;

        private int decreaseTextSize = 5;

        public PlayWindow(MenuWindow menuWindowReference, FlashcardOptions flashcardOptionsReference, FlashcardSet flashcardSet)
        {
            InitializeComponent();
            this.flashcardSet = CloneFlashcardSet(flashcardSet);
            flashcardDesign = new FlashcardDesign(false, false, incrementTextSize, decreaseTextSize);

            shuffle(this.flashcardSet.Flashcards);

            DataContext = this.flashcardSet;
            nameTextBox.Text = flashcardSet.FlashcardSetName;

            if (ListBoxFlashcards.Items.Count > 0)
            {
                DisplayFlashcard(currentFlashcardIndex);
            }
        }

        private void shuffle(ObservableCollection<Flashcard> flashcards)
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
                    FlashcardAnswer = flashcard.FlashcardAnswer,
                    FlashcardColor = flashcard.FlashcardColor
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
                string flashcardColorT = flashcardSet.Flashcards[index].FlashcardColor.ToString();
                int indexOfColon = flashcardColorT.IndexOf(":");

                if (indexOfColon != -1)
                {
                    flashcardColorT = flashcardColorT.Substring(indexOfColon + 2);
                }

                if (!string.IsNullOrEmpty(flashcardSet.Flashcards[index].FlashcardColor))
                {
                    SolidColorBrush colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcardColorT);

                    questionTextBox.Background = colorBrush;
                }

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
            DisplayAnswer(currentFlashcardIndex - 1);
        }

        private void HighlightText(object sender, RoutedEventArgs e)
        {
            flashcardDesign.IsHighlighted = !flashcardDesign.IsHighlighted;

            if(flashcardDesign.IsHighlighted == true)
            {
                questionTextBox.FontWeight = FontWeights.Bold;
                answerTextBox.FontWeight = FontWeights.Bold;
                HighlightButton.IsChecked = true;
            }
            else
            {
                questionTextBox.FontWeight = FontWeights.Normal;
                answerTextBox.FontWeight = FontWeights.Normal;
                HighlightButton.IsChecked = false;
            }
        }

        private void ItalicText(object sender, RoutedEventArgs e)
        {
            flashcardDesign.IsItalic = !flashcardDesign.IsItalic;
            
            if(flashcardDesign.IsItalic == true)
            {
                questionTextBox.FontStyle = FontStyles.Italic;
                answerTextBox.FontStyle = FontStyles.Italic;
                ItalicButton.IsChecked = true;
            }
            else
            {
                questionTextBox.FontStyle = FontStyles.Normal;
                answerTextBox.FontStyle = FontStyles.Normal;
                ItalicButton.IsChecked = false;
            }
        }

        private void UpTextSize(object sender, RoutedEventArgs e)
        {
            questionTextBox.FontSize += flashcardDesign.IncreaseTextSize;
            answerTextBox.FontSize += flashcardDesign.IncreaseTextSize;
            UpTextButton.IsChecked = true;
            DecTextButton.IsChecked = false;
        }

        private void DecTextSize(object sender, RoutedEventArgs e)
        {
            questionTextBox.FontSize -= flashcardDesign.DecreaseTextSize;
            answerTextBox.FontSize -= flashcardDesign.DecreaseTextSize;
            DecTextButton.IsChecked = true;
            UpTextButton.IsChecked = false;
        }
    }
}
>>>>>>> Stashed changes
