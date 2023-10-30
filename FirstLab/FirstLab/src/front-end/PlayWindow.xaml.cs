﻿using System;
using System.Collections;
using FirstLab.src.back_end;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using FirstLab.src.back_end.factories.factoryInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FirstLab.XAML
{
    public partial class PlayWindow : UserControl
    {
        private FlashcardSet flashcardSet;

        private ArrayList numbersOfFlashcards;

        private FlashcardDesign flashcardDesign;

        IServiceProvider serviceProvider;

        IFactoryContainer factoryContainer;

        private int currentFlashcardIndex = 0;

        private int incrementTextSize = 5;

        private int decreaseTextSize = 5;

        private int counter;

        private string StringCounter;

        private Thread timerThread;

        private readonly object lockObject;

        public PlayWindow(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.factoryContainer = factoryContainer;
            lockObject = factoryContainer.CreateObject<object>();
            this.flashcardSet = CloneFlashcardSet(flashcardSet);
            flashcardDesign = factoryContainer.CreateDesign(false, false, incrementTextSize, decreaseTextSize);

            Shuffle(this.flashcardSet.Flashcards);

            numbersOfFlashcards = new ArrayList();
            PopulateArray(this.flashcardSet.Flashcards);

            DataContext = this.flashcardSet;
            nameTextBox.Text = flashcardSet.FlashcardSetName;

            if (ListBoxFlashcards.Items.Count > 0)
            {
                DisplayFlashcard(currentFlashcardIndex);
            }
        }

        private void PopulateArray(ObservableCollection<Flashcard> flashcards)
        {
            for (int i = 1; i <= flashcards.Count(); i++)
            {
                numbersOfFlashcards.Add(i);
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
            FlashcardSet clonedSet = factoryContainer.CreateObject<FlashcardSet>();
            foreach (var flashcard in originalSet.Flashcards)
            {
                clonedSet.Flashcards.Add(flashcard);
            }
            clonedSet.FlashcardSetName = originalSet.FlashcardSetName;
            return clonedSet;
        }

        private void DisplayFlashcard(int index)
        {
            if (index >= 0 && index < ListBoxFlashcards.Items.Count)
            {
                flashcardNumberTextBlock.Text = ((int)numbersOfFlashcards[index]).ToString() + "/" + ListBoxFlashcards.Items.Count.ToString();
                questionTextBox.Text = flashcardSet.Flashcards[index].FlashcardQuestion;
                string flashcardColorT = flashcardSet.Flashcards[index].FlashcardColor.ToString();
                int indexOfColon = flashcardColorT.IndexOf(":");

                if (indexOfColon != -1)
                {
                    flashcardColorT = flashcardColorT.Substring(indexOfColon + 2);
                }

                if (!string.IsNullOrEmpty(flashcardSet.Flashcards[index].FlashcardColor))
                {
                    SolidColorBrush colorBrush = (SolidColorBrush) new BrushConverter().ConvertFromString(flashcardColorT);

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

        private void DisplayFlashcard(object sender, RoutedEventArgs e)
        {
            StringCounter = flashcardSet.Flashcards[currentFlashcardIndex].FlashcardTimer.ToString();
            timerListBox_SelectionChanged(StringCounter);
            DisplayFlashcard(currentFlashcardIndex);
            currentFlashcardIndex++;
            if (currentFlashcardIndex <= flashcardSet.Flashcards.Count)
            {
                InitTimer();
            }
        }

        private void DisplayAnswer(object sender, RoutedEventArgs e)
        {
            DisplayAnswer(currentFlashcardIndex - 1);
        }

        private void HighlightText(object sender, RoutedEventArgs e)
        {
            flashcardDesign.IsHighlighted = !flashcardDesign.IsHighlighted;

            if (flashcardDesign.IsHighlighted == true)
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

            if (flashcardDesign.IsItalic == true)
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

        private void InitTimer()
        {
            timerThread = new Thread(Countdown);
            timerThread.Start();
        }

        private void Countdown()
        {
            while (counter > 0)
            {
                lock (lockObject)
                {
                    counter--;

                    Dispatcher.Invoke(() =>
                    {
                        timerTextBox.Text = counter.ToString();
                    });
                }
                Thread.Sleep(1000);
            }

            if (counter == 0)
            {
                Dispatcher.Invoke(() =>
                {
                    DisplayAnswer(currentFlashcardIndex - 1);
                });
            }
        }

        private void timerListBox_SelectionChanged(string time)
        {
            string selectedTime = time.ToString();

            if (!string.IsNullOrEmpty(selectedTime))
            {
                counter = ExtractNumber(selectedTime);
            }
        }

        private int ExtractNumber(string input)
        {
            string numericPart = new string(input.Where(char.IsDigit).ToArray());

            if (int.TryParse(numericPart, out int timerCounter))
            {
                return timerCounter;
            }
            return 0;
        }
    }
}