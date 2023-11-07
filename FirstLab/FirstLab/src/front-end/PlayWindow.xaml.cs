using System;
using FirstLab.src.back_end;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using FirstLab.src.back_end.factories.factoryInterfaces;
using FirstLab.src.back_end.errorHandling;
using FirstLab.src;

namespace FirstLab.XAML;

public delegate void ActionDelegates();

public partial class PlayWindow : UserControl
{
    private FlashcardSet flashcardSet;

    private FlashcardDesign flashcardDesign;

    private int currentFlashcardIndex = 0, counter;

    private object lockObject;

    private bool isFunctioning;

    private ActionDelegates setTimer, showAnswer;

    IPlayWindowService _controllerService;

    public PlayWindow(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService controllerService)
    {
        InitializeComponent();
        InitializePlayWindowFields(flashcardSet, factoryContainer, controllerService);
        InitializeDelegates();
        controllerService.ShuffleFlashcards(this.flashcardSet!.Flashcards!);
        DisplayFlashcard();
    }

    private void InitializePlayWindowFields(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService controllerService)
    {
        _controllerService = controllerService;
        lockObject = factoryContainer.CreateObject<object>();
        this.flashcardSet = _controllerService.CloneFlashcardSet(flashcardSet);
        flashcardDesign = factoryContainer.CreateDesign(false, false, 5, 5);
        nameTextBox.Text = flashcardSet.FlashcardSetName;
    }

    private void InitializeDelegates()
    {
        setTimer = () =>
        {
            timerTextBox.Text = counter.ToString();
        };

        showAnswer = () =>
        {
            isFunctioning = false;
            DisplayAnswer();
        };
    }

    private void DisplayFlashcard(int index)
    {
        if (!_controllerService.IsIndexOverBounds(index, flashcardSet))
        {
            SetProperties(index);
            answerTextBox.Clear();
        }
    }

    private void SetProperties(int index)
    {
        flashcardNumberTextBlock.Text = (index+1).ToString() + "/" + flashcardSet.Flashcards!.Count().ToString();
        questionTextBox.Text = flashcardSet.Flashcards![index].FlashcardQuestion;
        try
        {
            questionTextBox.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcardSet.Flashcards![index].FlashcardColor!)!;
        }
        catch (Exception ex)
        {
            _controllerService.ThrowCustomException($"No default color has been selected", ex);
        }
    }

    private void DisplayFlashcard(object? sender = null, RoutedEventArgs? e = null)
    {
        if (!isFunctioning)
        {
            try
            {
                counter = _controllerService.SetTheCounter(currentFlashcardIndex, flashcardSet);
            }
            catch (CustomNullException ex)
            {
                if (!_controllerService.IsIndexOverBounds(currentFlashcardIndex, flashcardSet))
                {
                    _controllerService.HandleNullTimer(ex, flashcardSet, currentFlashcardIndex);
                    counter = _controllerService.SetTheCounter(currentFlashcardIndex, flashcardSet);
                }
                else
                {
                    _controllerService.LogCustomException($"Error. All flashcards have been displayed");
                }
            }

            try
            {
                DisplayFlashcard(currentFlashcardIndex);
            }
            catch (CustomNullException ex)
            {
                _controllerService.HandleNullColor(ex, flashcardSet, currentFlashcardIndex);
                DisplayFlashcard(currentFlashcardIndex);
            }

            currentFlashcardIndex++;

            if (currentFlashcardIndex <= flashcardSet.Flashcards!.Count)
            {
                InitTimer();
            }
        }
    }

    private void DisplayAnswer(object? sender = null, RoutedEventArgs? e = null)
    {
        try
        {
            isFunctioning = false;
            answerTextBox.Text = flashcardSet.Flashcards![currentFlashcardIndex - 1].FlashcardAnswer;
        }
        catch
        {
            _controllerService.LogCustomException($"Error displaying flashcard answer");
        }
    }

    private void ChangeText(bool isHighlighted, bool isItalic)
    {
        flashcardDesign.IsHighlighted = isHighlighted;
        flashcardDesign.IsItalic = isItalic;
        HighlightButton.IsChecked = isHighlighted;
        ItalicButton.IsChecked = isItalic;

        FontWeight fontWeight = isHighlighted ? FontWeights.Bold : FontWeights.Normal;
        FontStyle fontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal;

        questionTextBox.FontWeight = fontWeight;
        answerTextBox.FontWeight = fontWeight;
        questionTextBox.FontStyle = fontStyle;
        answerTextBox.FontStyle = fontStyle;
    }

    private void HighlightText(object sender, RoutedEventArgs e)
    {
        ChangeText(!flashcardDesign.IsHighlighted, flashcardDesign.IsItalic);
    }

    private void ItalicText(object sender, RoutedEventArgs e)
    {
        ChangeText(flashcardDesign.IsHighlighted, !flashcardDesign.IsItalic);
    }

    private void ChangeTextSize(object sender, RoutedEventArgs e)
    {
        bool increaseSize = (sender == UpTextButton);
        double sizeChange = increaseSize ? flashcardDesign.IncreaseTextSize : -flashcardDesign.DecreaseTextSize;

        questionTextBox.FontSize += sizeChange;
        answerTextBox.FontSize += sizeChange;
        UpTextButton.IsChecked = increaseSize;
        DecTextButton.IsChecked = !increaseSize;
    }

    private void InitTimer()
    {
        Thread timerThread = new (Countdown);
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

                Dispatcher.Invoke(setTimer);
            }
            finally
            {
                Monitor.Exit(lockObject);
            }

            Thread.Sleep(1000);
        }

        if (counter == 0)
        {
            Dispatcher.Invoke(showAnswer);
        }
    }
}