using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstLab.src.errorHandling;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.utilities;

namespace FirstLab;

public partial class FlashcardCustomization : UserControl
{
    private FlashcardSet flashcardSet;

    private FlashcardOptions flashcardOptionsReference;

    private string? NameOfSet;

    IFactoryContainer factoryContainer;

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
        this.factoryContainer = factoryContainer;
        this.flashcardSet = flashcardSet ?? factoryContainer.CreateObject<FlashcardSet>();
        DataContext = this.flashcardSet;
        this.PreviewKeyDown += UserControl_PreviewKeyDown;
    }

    private async void CheckIfEditingOrNew(FlashcardSet? flashcardSet)
    {
        if (flashcardSet == null)
        {
            AddFlashcard_Click();
        }
        else
        {
            await _ifFlashcardCustomizationService.RemoveSetFromDatabase(flashcardSet, flashcardOptionsReference);
            ListBoxFlashcards.SelectedIndex = flashcardSet.Flashcards!.Count - 1;
            NameOfSet = flashcardSet.FlashcardSetName;
        }
    }

    private void IsQestionOrAnswer(bool question, bool answer)
    {
        if(!question && !answer)
        {
            QuestionTextBox.IsEnabled = false;
            AnswerTextBox.IsEnabled = false;
            QuestionBorder.Visibility = Visibility.Collapsed;
            AnswerBorder.Visibility = Visibility.Collapsed;
            QuestionRadioButton.Visibility = Visibility.Collapsed;
            AnswerRadioButton.Visibility = Visibility.Collapsed;
            ColorBox.Visibility = Visibility.Collapsed; 
            timerListBox.Visibility = Visibility.Collapsed;
        }
        else if(question && !answer)
        {
            QuestionBorder.Visibility = Visibility.Visible;
            QuestionRadioButton.Visibility = Visibility.Visible;
            QuestionRadioButton.IsChecked = true;
            QuestionTextBox.Visibility = Visibility.Visible;
            QuestionTextBox.IsEnabled = true;
            AnswerBorder.Visibility = Visibility.Collapsed;
            AnswerRadioButton.Visibility = Visibility.Visible;
            ColorBox.Visibility= Visibility.Visible;
            timerListBox.Visibility = Visibility.Visible;
            QuestionTextBox.Focus();
        }
        else if(!question && answer)
        {
            QuestionBorder.Visibility = Visibility.Collapsed;
            AnswerBorder.Visibility = Visibility.Visible;
            QuestionTextBox.IsEnabled = false;
            AnswerTextBox.IsEnabled = true;
            AnswerRadioButton.IsChecked = true;
            AnswerTextBox.Focus();
        }
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
        ListBoxFlashcards.SelectedIndex = (oldIndex - 1 < 0) ? 0 : oldIndex - 1;
    }

    private void QuestionAnswerRadioButton_Click(object sender, RoutedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void CapitalizedNormalNameButton_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        if (CapitalizeButton.IsChecked == true)
        {
            NameOfSet = FlashcardSetNameBox.Text;
            FlashcardSetNameBox.Text = NameOfSet.Capitalize();
            _ifFlashcardCustomizationService.SaveFlashcardSetName(NameOfSet.Capitalize(), flashcardSet);
        }
        else
        {
            FlashcardSetNameBox.Text = NameOfSet;
            _ifFlashcardCustomizationService.SaveFlashcardSetName(NameOfSet!, flashcardSet);
        }
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

    private async Task SaveFlashcards()
    {
        if (IsFlashcardSetCorrect())
        {
            await _ifFlashcardCustomizationService.SaveToDatabase(flashcardSet, flashcardOptionsReference);
            ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
        }
    }
    private async void SaveFlashcardSet_Click(object sender, RoutedEventArgs e)
    {
        await SaveFlashcards();
    }

    private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void TimerListBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private bool IsFlashcardSetCorrect()
    {
        CustomizationErrors errors = InitializeErrors();
        errors.CheckAndDisplayErrors();
        return !errors.ErrorCodes.Any();
    }

    private CustomizationErrors InitializeErrors()
    {
        return factoryContainer.CreateErrorHandling(
            flashcardSet: flashcardSet,
            nameOfFlashcardSet: FlashcardSetNameBox.Text,
            errorTextBox: errorText,
            SetsOfFlashcards: flashcardOptionsReference.flashcardSets
        );
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
                SaveFlashcards();
                break;
            case Key.Escape:
                ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
                break;
        }
    }

    private void NavigateFlashcards(int direction)
    {
        int currentIndex = ListBoxFlashcards.SelectedIndex;

        if (currentIndex >= 0 && currentIndex < flashcardSet.Flashcards.Count)
        {
            int newIndex = currentIndex + direction;
            if (newIndex >= 0 && newIndex < flashcardSet.Flashcards.Count)
            {
                ListBoxFlashcards.SelectedIndex = newIndex;
                IsQestionOrAnswer(true, false);
            }
        }
    }
}
