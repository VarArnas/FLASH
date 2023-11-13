using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.exceptions;
using System.Windows.Input;
using System.Windows.Media.Animation;
using FirstLab.src.utilities;
using System.Windows.Shapes;

namespace FirstLab.XAML;

public delegate void ActionDelegates();

public partial class PlayWindow : Window
{
    private FlashcardSet flashcardSet;

    private FlashcardDesign flashcardDesign;

    private int currentFlashcardIndex = 0, counter;

    private object lockObject;

    private bool isFunctioning;

    private bool switchDisplay;

    private ActionDelegates setTimer, showAnswer;

    private bool isPanelVisible = true;

    IPlayWindowService _controllerService;

    public PlayWindow(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService controllerService)
    {
        InitializeComponent();
        InitializePlayWindowFields(flashcardSet, factoryContainer, controllerService);
        InitializeDelegates();
        controllerService.ShuffleFlashcards(this.flashcardSet!.Flashcards!);
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {

        QuestionBorder.Visibility = Visibility.Visible;
        AnswerBorder.Visibility = Visibility.Collapsed;
        timerTextBox.Focus();
        switchDisplay = true;

        DoubleAnimation opacityAnimation = new DoubleAnimation();
        opacityAnimation.From = 1.0;
        opacityAnimation.To = 0.1;
        opacityAnimation.Duration = TimeSpan.FromSeconds(2);
        opacityAnimation.AutoReverse = true;
        opacityAnimation.RepeatBehavior = RepeatBehavior.Forever;
        breathingEllipse.BeginAnimation(Ellipse.OpacityProperty, opacityAnimation);
    }

    private void InitializePlayWindowFields(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService controllerService)
    {
        _controllerService = controllerService;
        lockObject = factoryContainer.CreateObject<object>();
        this.flashcardSet = _controllerService.CloneFlashcardSet(flashcardSet);
        flashcardDesign = factoryContainer.CreateDesign(false, false, 5, 5);
        this.PreviewKeyDown += UserControl_PreviewKeyDown;
    }

    private void InitializeDelegates()
    {
        setTimer = () =>
        {
            timerTextBox.Text = counter.ToString() + "s";
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
            QuestionBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcardSet.Flashcards![index].FlashcardColor!)!;
            AnswerBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcardSet.Flashcards![index].FlashcardColor!)!;
            QuestionBorder.Visibility = Visibility.Visible;
            AnswerBorder.Visibility = Visibility.Collapsed;
        }
        catch (Exception ex)
        {
            _controllerService.ThrowCustomException($"No default color has been selected", ex);
        }
    }
    private void DisplayFlashcard(object? sender = null, RoutedEventArgs? e = null)
    {
        lock (lockObject)
        {
            if (switchDisplay)
            {
                switchDisplay = false;

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
        }
    }

    private void DisplayAnswer(object? sender = null, RoutedEventArgs? e = null)
    {
        lock (lockObject) //?????????????
        {
            if(!switchDisplay)
            {
                try
                {
                   answerTextBox.Text = flashcardSet.Flashcards![currentFlashcardIndex - 1].FlashcardAnswer;
                   QuestionBorder.Visibility = Visibility.Collapsed;
                   AnswerBorder.Visibility = Visibility.Visible;
                }
                catch
                {
                    _controllerService.LogCustomException($"Error displaying flashcard answer");
                }
            }

            switchDisplay = true;
            isFunctioning = false;
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

    private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.A:
                DisplayFlashcard();
                break;

            case Key.D:
                DisplayAnswer();
                break;

            case Key.Escape:
                CloseCommand();
                break;
        }
    }

    private void CloseCommand()
    {
        ViewsUtils.menuWindowReference!.ReturnToHomeView_Click(this);
        this.Close();
        ViewsUtils.menuWindowReference!.Show();
    }

    private void MovingWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void SlidePanelButton_Click(object sender, RoutedEventArgs e)
    {
        isPanelVisible = !isPanelVisible;
        var storyboardName = isPanelVisible ? "SlideInAnimation" : "SlideOutAnimation";
        var storyboard = (Storyboard)Resources[storyboardName];
        storyboard.Begin();
    }

}