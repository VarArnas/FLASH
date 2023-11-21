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

namespace FirstLab.XAML;

public partial class PlayWindow : Window
{
    private FlashcardSet flashcardSet;

    private Flashcard currentFlashcard;

    private FlashcardDesign flashcardDesign;

    private int counter, incrementTextBy = 5, decrementTextBy = 5;

    private bool isAnswerDisplayed = true, isItalic = false, isBold = false, isStart = true;

    IPlayWindowService _playWindowService;

    private CancellationTokenSource cancellationTokenSource;

    Storyboard? storyboard;

    public PlayWindow(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService playWindowService)
    {
        InitializeComponent();
        InitializePlayWindowFields(flashcardSet, factoryContainer, playWindowService);
        playWindowService.ShuffleFlashcards(this.flashcardSet!.Flashcards!);
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
        if(!_playWindowService.isFirstOrZeroIndex(HiddenFlashcardSetListBox.SelectedIndex))
            DisplayFlashcard(true);
    }

    private void DisplayNextFlashcard_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if(!_playWindowService.isLastIndex(HiddenFlashcardSetListBox.SelectedIndex, flashcardSet))
            DisplayFlashcard(false);
    }

    private void DisplayFlashcard(bool isPreviousFlashcardNeeded)
    {
        if(isAnswerDisplayed)
        {
            HiddenFlashcardSetListBox.SelectedIndex = _playWindowService.CheckIfPreviousOrNext(isPreviousFlashcardNeeded, HiddenFlashcardSetListBox.SelectedIndex, flashcardSet, isStart);
            storyboard = isPreviousFlashcardNeeded ? FindResource("BounceEffectAnimationOut") as Storyboard : FindResource("BounceEffectAnimation") as Storyboard;
            SetStatesForQuestion();
            currentFlashcard = (Flashcard)HiddenFlashcardSetListBox.SelectedItem;
            _playWindowService.CreateCounter(ref counter, currentFlashcard);
            storyboard!.Begin();
            MapQuestionAnswerProperties(_playWindowService.GetQuestionAnswerProperties(true, false, currentFlashcard, flashcardSet));

            if (!_playWindowService.isFirstOrZeroIndex(counter))
                InitTimer();
        }
    }

    private void DisplayAnswer_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if(!isAnswerDisplayed)
        {
            cancellationTokenSource?.Cancel();
            FlashcardAnimation(_playWindowService.SetQuestionOrAnswerProperties(false, true, currentFlashcard, flashcardSet));
            isAnswerDisplayed = true;
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
        cancellationTokenSource?.Cancel();
        ViewsUtils.menuWindowReference!.ReturnToHomeView_Click(this);
        this.Close();
        ViewsUtils.menuWindowReference!.Show();
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
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
        Thread timerThread = new Thread(() => Countdown(cancellationTokenSource.Token));
        timerThread.Start();
    }

    private void Countdown(CancellationToken cancellationToken)
    {
        while (counter > 0)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            counter--;
            Dispatcher.Invoke(UpdateTimerUI);
            Thread.Sleep(1000);
        }
        if (counter == 0 && !cancellationToken.IsCancellationRequested)
            Dispatcher.Invoke(() => DisplayAnswer_Click());
    }

    private void UpdateTimerUI()
    {
        timerTextBox.Text = counter.ToString() + "s";
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