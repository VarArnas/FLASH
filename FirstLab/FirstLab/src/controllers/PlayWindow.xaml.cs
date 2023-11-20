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

    private FlashcardDesign flashcardDesign;

    private int currentFlashcardIndex = 0, counter, incrementTextBy = 5, decrementTextBy = 5;

    private bool isFunctioning = false, isItalic = false, isBold = false;

    IPlayWindowService _playWindowService;

    private CancellationTokenSource cancellationTokenSource;


    public PlayWindow(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService playWindowService)
    {
        InitializeComponent();
        InitializePlayWindowFields(flashcardSet, factoryContainer, playWindowService);
        playWindowService.ShuffleFlashcards(this.flashcardSet!.Flashcards!);
    }

    private void PlayWindow_Loaded(object sender, RoutedEventArgs e)
    {
        timerTextBox.Focus();
        QuestionBorder.MouseLeftButtonDown -= DisplayAnswer_Click;
        AnswerBorder.MouseLeftButtonDown -= DisplayAnswer_Click;
        breathingEllipse.BeginAnimation(Ellipse.OpacityProperty, _playWindowService.SetAnimation());
    }

    private void InitializePlayWindowFields(FlashcardSet flashcardSet, IFactoryContainer factoryContainer, IPlayWindowService playWindowService)
    {
        _playWindowService = playWindowService;
        this.flashcardSet = _playWindowService.CloneFlashcardSet(flashcardSet);
        flashcardDesign = factoryContainer.CreateDesign(isItalic, isBold, incrementTextBy, decrementTextBy);
        DataContext = this.flashcardSet;
        PreviewKeyDown += UserControl_PreviewKeyDown;
    }

    private void DisplayFlashcard_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if (!isFunctioning && (currentFlashcardIndex != flashcardSet.Flashcards!.Count() || currentFlashcardIndex == 0))
        {
            _playWindowService.CreateCounter(ref counter, currentFlashcardIndex, flashcardSet);
            var properties = _playWindowService.GetQuestionAnswerProperties(true, false, currentFlashcardIndex, flashcardSet);
            Storyboard slideInStoryboard = FindResource("BounceEffectAnimation") as Storyboard;
            slideInStoryboard?.Begin();
            MapQuestionAnswerProperties(properties);
            _playWindowService.TryToIncrementCurrentIndex(ref currentFlashcardIndex, flashcardSet);
            QuestionBorder.MouseLeftButtonDown += DisplayAnswer_Click;
            AnswerBorder.MouseLeftButtonDown += DisplayAnswer_Click;
            if(counter != 0)
                InitTimer();
            else
                isFunctioning = true;
        }
    }

    private void DisplayAnswer_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        cancellationTokenSource?.Cancel();
        var properties = _playWindowService.SetQuestionOrAnswerProperties(false, true, currentFlashcardIndex, flashcardSet);
        QuestionBorder.MouseLeftButtonDown -= DisplayAnswer_Click;
        AnswerBorder.MouseLeftButtonDown -= DisplayAnswer_Click;
        isFunctioning = false;
        FlashcardAnimation(properties);
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
                DisplayFlashcard_Click();
                break;

            case Key.D:
                if (isFunctioning)
                    DisplayAnswer_Click();
                break;

            case Key.Escape:
                CloseCommand();
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
        isFunctioning = true;
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
}