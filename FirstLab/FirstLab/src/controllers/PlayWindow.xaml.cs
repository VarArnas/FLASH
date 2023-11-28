using System.Windows;
using System.Threading;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using System.Windows.Input;
using System.Windows.Media.Animation;
using FirstLab.src.utilities;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Media;
using System;

namespace FirstLab.src.controllers;

public partial class PlayWindow : Window
{
    private FlashcardSet flashcardSet;

    private Flashcard currentFlashcard;

    private FlashcardDesign flashcardDesign;

    private int counter, incrementTextBy = 5, decrementTextBy = 5;

    private bool isAnswerDisplayed = true, isItalic = false, isBold = false, isStart = true;

    IPlayWindowService _playWindowService;

    Storyboard? storyboard;

    private DispatcherTimer? countdownTimer;


    public PlayWindow(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService playWindowService)
    {
        InitializeComponent();
        InitializePlayWindowFields(flashcardSet, factoryContainer, playWindowService);
        playWindowService.ShuffleFlashcards(this.flashcardSet!.Flashcards!);
        InitializeTimer();

    }

    private void InitializeTimer()
    {
        countdownTimer = new DispatcherTimer();
        countdownTimer.Interval = TimeSpan.FromSeconds(1);
        countdownTimer.Tick += CountdownTimer_Tick!;
    }

    private void PlayWindow_Loaded(object sender, RoutedEventArgs e)
    {
        timerTextBox.Focus();
        breathingEllipse.BeginAnimation(Ellipse.OpacityProperty, _playWindowService.SetAnimation());
    }

    private void InitializePlayWindowFields(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService playWindowService)
    {
        _playWindowService = playWindowService;
        this.flashcardSet = _playWindowService.CloneFlashcardSet(flashcardSet);
        flashcardDesign = factoryContainer.CreateDesign(isItalic, isBold, incrementTextBy, decrementTextBy);
        DataContext = this.flashcardSet;
        difficultyField.Text += flashcardSet.FlashcardSetDifficulty;
        HiddenFlashcardSetListBox.SelectedIndex = 0;
        PreviewKeyDown += UserControl_PreviewKeyDown;
    }

    private void DisplayPreviousFlashcard_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if(!_playWindowService.IsFirstOrZeroIndex(HiddenFlashcardSetListBox.SelectedIndex))
            DisplayFlashcard(true);
    }

    private void DisplayNextFlashcard_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if(!_playWindowService.IsLastIndex(HiddenFlashcardSetListBox.SelectedIndex, flashcardSet))
            DisplayFlashcard(false);
    }

    private void DisplayFlashcard(bool isPreviousFlashcardNeeded)
    {
        countdownTimer!.Stop();
        HiddenFlashcardSetListBox.SelectedIndex = _playWindowService.CheckIfPreviousOrNext(isPreviousFlashcardNeeded, HiddenFlashcardSetListBox.SelectedIndex, flashcardSet, isStart);
        storyboard = isPreviousFlashcardNeeded ? FindResource("BounceEffectAnimationOut") as Storyboard : FindResource("BounceEffectAnimation") as Storyboard;
        SetStatesForQuestion();
        currentFlashcard = (Flashcard)HiddenFlashcardSetListBox.SelectedItem;
        _playWindowService.CreateCounter(ref counter, currentFlashcard);
        storyboard!.Begin();
        MapQuestionAnswerProperties(_playWindowService.GetQuestionAnswerProperties(true, false, currentFlashcard, flashcardSet));

        if (!_playWindowService.IsFirstOrZeroIndex(counter))
            InitTimer();
    }

    private void DisplayAnswer_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if(!isAnswerDisplayed)
        {
            FlashcardAnimation(_playWindowService.GetQuestionAnswerProperties(false, true, currentFlashcard, flashcardSet));
            isAnswerDisplayed = true;
            countdownTimer!.Stop();
        }
    }


    private void HighlightText_Click(object sender, RoutedEventArgs e)
    {
        ChangeQuestionAnswerTextProperties(!flashcardDesign.IsHighlighted, flashcardDesign.IsItalic);
    }

    private void ItalicText_Click(object sender, RoutedEventArgs e)
    {
        ChangeQuestionAnswerTextProperties(flashcardDesign.IsHighlighted, !flashcardDesign.IsItalic);
    }

    private void ChangeTextSize_Click(object sender, RoutedEventArgs e)
    {
        var size = _playWindowService.FindNewTextSize(sender == UpTextButton, flashcardDesign, questionTextBox.FontSize);
        questionTextBox.FontSize = size;
        answerTextBox.FontSize = size;
    }
    private void MovingWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void SlidePanelButton_Click(object sender, RoutedEventArgs e)
    {
        ((Storyboard)Resources[_playWindowService.SetSlidePanelAnimation()]).Begin();
    }

    private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.A:
                DisplayPreviousFlashcard_Click();
                break;

            case Key.D:
                DisplayNextFlashcard_Click();
                break;

            case Key.Escape:
                CloseCommand();
                break;

            case Key.Space:
                DisplayAnswer_Click();
                break;
        }
    }

    private void CloseCommand()
    {
        StopAndResetTimer();
        ViewsUtils.menuWindowReference!.ReturnToHomeView_Click(this);
        if(ViewsUtils.menuWindowReference!.contentControl.Content is HomeView)
        {
            ViewsUtils.menuWindowReference!.Show();
            this.Close();
        }
    }

    private void ChangeQuestionAnswerTextProperties(bool isHighlighted, bool isItalic)
    {
        var textProperties = _playWindowService.SetTextProperties(isHighlighted, isItalic);
        flashcardDesign.IsHighlighted = textProperties.HighlightBtn;
        flashcardDesign.IsItalic = textProperties.ItalicBtn;
        HighlightButton.IsChecked = textProperties.HighlightBtn;
        ItalicButton.IsChecked = textProperties.ItalicBtn;
        questionTextBox.FontWeight = textProperties.QuestionAnswerTextWeight;
        answerTextBox.FontWeight = textProperties.QuestionAnswerTextWeight;
        questionTextBox.FontStyle = textProperties.QuestionAnswerTextStyle;
        answerTextBox.FontStyle = textProperties.QuestionAnswerTextStyle;
    }

    private void MapQuestionAnswerProperties(TextAndBorderPropertiesPlayWindow properties)
    {
        flashcardNumberTextBlock.Text = properties.FlashcardNumberText;
        QuestionBorder.Background = properties.BorderColor;
        AnswerBorder.Background = properties.BorderColor;
        QuestionBorder.Visibility = properties.QuestionBorderVisibility;
        AnswerBorder.Visibility = properties.AnswerBorderVisibility;
        (QuestionBorder.Visibility == Visibility.Visible ? questionTextBox : answerTextBox).Text = properties.QuestionAnswerText;
    }

    private void InitTimer()
    {
        UpdateTimerUI();
        countdownTimer!.Start();
    }

    private void CountdownTimer_Tick(object sender, EventArgs e)
    {
        if (counter > 0)
        {
            counter--;
            UpdateTimerUI();
        }
        else
        {
            countdownTimer!.Stop();
            DisplayAnswer_Click();
        }
    }

    private void UpdateTimerUI()
    {
        timerTextBox.Text = counter.ToString() + "s";
    }

    private void StopAndResetTimer()
    {
        countdownTimer!.Stop();
        countdownTimer.Tick -= CountdownTimer_Tick!;
        countdownTimer = null;
    }

    private void FlashcardAnimation(TextAndBorderPropertiesPlayWindow properties)
    {
        var flipQuestionOutStoryboard = FindResource("FlipQuestionOutAnimation") as Storyboard;
        var flipAnswerInStoryboard = FindResource("FlipAnswerInAnimation") as Storyboard;

        flipQuestionOutStoryboard!.Completed += (s, args) =>
        {
            MapQuestionAnswerProperties(properties);
            flipAnswerInStoryboard!.Begin();
        };

        flipAnswerInStoryboard!.Completed += (s, args) =>
        {
            flipQuestionOutStoryboard.Stop();
            Dispatcher.Invoke(RevertScaleOfQuestion);
        };

        flipQuestionOutStoryboard!.Begin();
    }

    private void RevertScaleOfQuestion()
    {
        if (QuestionBorder.RenderTransform is ScaleTransform scaleTransform)
            scaleTransform.ScaleX = 1;
        else
            QuestionBorder.RenderTransform = new ScaleTransform(1, 1);
    }

    private void SetStatesForQuestion()
    {
        isAnswerDisplayed = false;
        isStart = false;
        timerTextBox.Text = string.Empty;
    }

    private void Button_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
            e.Handled = true;
    }
}