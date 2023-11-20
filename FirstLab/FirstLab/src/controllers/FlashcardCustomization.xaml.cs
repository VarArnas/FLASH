using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.utilities;

namespace FirstLab;

public partial class FlashcardCustomization : UserControl
{
    private FlashcardSet flashcardSet;

    private FlashcardOptions flashcardOptionsReference;

    private string? NameOfSet;

    IFlashcardCustomizationService _ifFlashcardCustomizationService;

    public FlashcardCustomization(FlashcardOptions flashcardOptionsReference, IFactoryContainer factoryContainer,
        IFlashcardCustomizationService ifFlashcardCustomizationService, FlashcardSet? flashcardSet = null)
    {
        InitializeComponent();
        InitializeCustomizationFields(flashcardOptionsReference, factoryContainer, ifFlashcardCustomizationService, flashcardSet);
        CheckIfEditingOrNew(flashcardSet);
    }

    private void FlashcardCustmization_Loaded(object? sender = null, RoutedEventArgs? e = null)
    {
        QuestionTextBox.Focus();
    }

    private void InitializeCustomizationFields(FlashcardOptions flashcardOptionsReference, IFactoryContainer factoryContainer,
        IFlashcardCustomizationService ifFlashcardCustomizationService, FlashcardSet? flashcardSet = null)
    {
        _ifFlashcardCustomizationService = ifFlashcardCustomizationService;
        this.flashcardOptionsReference = flashcardOptionsReference;
        this.flashcardSet = flashcardSet ?? factoryContainer.CreateObject<FlashcardSet>();
        DataContext = this.flashcardSet;
        PreviewKeyDown += UserControl_PreviewKeyDown;
    }

    private void CheckIfEditingOrNew(FlashcardSet? flashcardSet)
    {
        if (!_ifFlashcardCustomizationService.CheckIfEditingAndRemoveTheOldFlashcardSet(flashcardSet, flashcardOptionsReference, NameOfSet))
            AddFlashcard_Click();
        else
            ListBoxFlashcards.SelectedIndex = flashcardSet!.Flashcards!.Count - 1;
    }

    private void AddFlashcard_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        int index = _ifFlashcardCustomizationService.AddFlashcard(flashcardSet);
        ListBoxFlashcards.Items.Refresh();
        ListBoxFlashcards.SelectedIndex = index;
        IsQestionOrAnswer(true, false);
    }

    private void ListBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       IsQestionOrAnswer(true, false);
    }

    private void DeleteFlashcard_Click(object sender, RoutedEventArgs e)    
    {
        int oldIndex = _ifFlashcardCustomizationService.DeleteFlashcard(ListBoxFlashcards.SelectedIndex, flashcardSet);
        ListBoxFlashcards.Items.Refresh();
        ListBoxFlashcards.SelectedIndex = _ifFlashcardCustomizationService.CalculateSelectionIndexAfterDeletion(oldIndex);
    }

    private void QuestionAnswerRadioButton_Click(object sender, RoutedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void CapitalizedNormalNameButton_Click(object? sender = null, RoutedEventArgs? e = null)
    {
       FlashcardSetNameBox.Text = _ifFlashcardCustomizationService.CapitalizeFlashcardSetName(CapitalizeButton.IsChecked, NameOfSet!, FlashcardSetNameBox.Text);
       _ifFlashcardCustomizationService.SaveFlashcardSetName(FlashcardSetNameBox.Text!, flashcardSet);
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        CapitalizeButton.IsChecked = false;
        NormalizeButton.IsChecked = true;
        CapitalizedNormalNameButton_Click();
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        NameOfSet = FlashcardSetNameBox.Text;
    }

    private void DeleteFlashcardSet_Click(object sender, RoutedEventArgs e)
    {
        ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
    }
    private async void SaveFlashcardSet_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        await _ifFlashcardCustomizationService.CheckErrorsAndSaveFlashcard(flashcardSet, FlashcardSetNameBox.Text, errorText,
            flashcardOptionsReference.flashcardSets, flashcardOptionsReference);
    }

    private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void TimerListBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                IsQestionOrAnswer(true, false);
                break;
            case Key.Right:
                IsQestionOrAnswer(false, true);
                break;
            case Key.Up:
                NavigateFlashcards(-1);
                break;
            case Key.Down:
                NavigateFlashcards(1);
                break;
            case Key.Enter:
                SaveFlashcardSet_Click();
                break;
            case Key.Escape:
                ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
                break;
        }
    }

    private void NavigateFlashcards(int direction)
    {
        ListBoxFlashcards.SelectedIndex = _ifFlashcardCustomizationService.CanYouChangeFlashcards(ListBoxFlashcards.SelectedIndex, flashcardSet, direction);
        IsQestionOrAnswer(true, false);
    }

    private void IsQestionOrAnswer(bool question, bool answer)
    {
        var model = _ifFlashcardCustomizationService.ChangeQuestionAnswerProperties(question, answer);
        QuestionBorder.Visibility = model._QuestionBorderVisibility;
        AnswerBorder.Visibility = model._AnswerBorderVisibility;
        QuestionRadioButton.IsChecked = model._CheckQuestionRadioButton;
        AnswerRadioButton.IsChecked = model._CheckAnswerRadioButton;
        (question ? QuestionTextBox : AnswerTextBox).Focus();
    }

    private static string ConvertColorToDifficulty(object value)
    {
        string difficulty = "Medium";
        if (value is SolidColorBrush colorBrush)
        {
            if (colorBrush.Color == Colors.Red)
                difficulty = "Very easy";
            else if (colorBrush.Color == Colors.Green)
                difficulty = "Easy";
            else if (colorBrush.Color == Colors.Yellow)
                difficulty = "Medium";
            else if (colorBrush.Color == Colors.Blue)
                difficulty = "Hard";
            else if (colorBrush.Color == Colors.Orange)
                difficulty = "Very hard";
        }

        return difficulty;
    }
}
