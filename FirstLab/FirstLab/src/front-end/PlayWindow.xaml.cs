using System;
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
using FirstLab.src.back_end.errorHandling;

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

        private string stringCounter;

        private Thread timerThread;

        private readonly object lockObject;

        private bool isFunctioning;

        private SelectionErrors selectionErrors;

        private string defaultColor = "System.Windows.Controls.ListBoxItem: Blue";

        private string defaultTimer = "System.Windows.Controls.ListBoxItem: 5 seconds";

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
                string flashcardColorT;

                try
                {
                    if (flashcardSet != null && flashcardSet.Flashcards != null && flashcardSet.Flashcards[index].FlashcardColor != null)
                    {
                        flashcardColorT = flashcardSet.Flashcards[index].FlashcardColor.ToString();
                    }
                    else
                    {
                        flashcardColorT = defaultColor;
                    }

                    int indexOfColon = flashcardColorT.IndexOf(":");

                    if (indexOfColon != -1)
                    {
                        flashcardColorT = flashcardColorT.Substring(indexOfColon + 2);
                    }

                    if (!string.IsNullOrEmpty(flashcardColorT))
                    {
                        SolidColorBrush colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcardColorT);

                        questionTextBox.Background = colorBrush;
                    }

                    answerTextBox.Clear();
                }
                catch (NullReferenceException ex)
                {
                    SelectionErrors displayError = new SelectionErrors($"No default color has been selected. Red color applied");
                    SelectionErrors.LogException(displayError);
                    SelectionErrors.LogException(ex);
                }
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
            try
            {
                if (flashcardSet != null && flashcardSet.Flashcards != null && flashcardSet.Flashcards[currentFlashcardIndex].FlashcardTimer != null)
                {
                    stringCounter = flashcardSet.Flashcards[currentFlashcardIndex].FlashcardTimer.ToString();
                }
                else
                {
                    flashcardSet.Flashcards[currentFlashcardIndex].FlashcardTimer = defaultTimer;
                }
            }
            catch (NullReferenceException ex)
            {
                SelectionErrors displayError = new SelectionErrors($"No default timer has been selected. 5 seconds applied");
                SelectionErrors.LogException(displayError);
                SelectionErrors.LogException(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                SelectionErrors displayError = new SelectionErrors($"Error. All flashcards have been displayed");
                SelectionErrors.LogException(displayError);
                SelectionErrors.LogException(ex);
            }

            if (!isFunctioning)
            {
                if (currentFlashcardIndex < flashcardSet.Flashcards.Count)
                {
                    stringCounter = flashcardSet.Flashcards[currentFlashcardIndex].FlashcardTimer;
                }

                timerListBox_SelectionChanged(stringCounter);
                DisplayFlashcard(currentFlashcardIndex);
                currentFlashcardIndex++;
                if (currentFlashcardIndex <= flashcardSet.Flashcards.Count)
                {
                    InitTimer();
                }
            }
        }

        private void DisplayAnswer(object sender, RoutedEventArgs e)
        {
            try
            {
                isFunctioning = false;
                DisplayAnswer(currentFlashcardIndex - 1);
            }
            catch (Exception ex)
            {
                SelectionErrors displayError = new SelectionErrors($"Error displaying flashcard answer");
                SelectionErrors.LogException(displayError);
                SelectionErrors.LogException(ex);
            }
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
            isFunctioning = true;

            while (counter > 0)
            {
                Monitor.Enter(lockObject);
                try
                {
                    if (!isFunctioning)
                    {
                        return;
                    }

                    counter--;

                    Dispatcher.Invoke(() =>
                    {
                        timerTextBox.Text = counter.ToString();
                    });
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                Thread.Sleep(1000);
            }

            if (counter == 0)
            {
                Dispatcher.Invoke(() =>
                {
                    isFunctioning = false;
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