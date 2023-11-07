using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstLab.src.back_end;
using FirstLab.src.back_end.data;
using FirstLab.src.back_end.errorHandling;
using FirstLab.src.back_end.factories.factoryInterfaces;
using FirstLab.src.back_end.utilities;

namespace FirstLab;

public partial class FlashcardCustomization : UserControl
{
    private FlashcardSet flashcardSet;

    private FlashcardOptions flashcardOptionsReference;

    private string? NameOfSet;

    IFactoryContainer factoryContainer;

    public FlashcardCustomization(FlashcardOptions flashcardOptionsReference, IFactoryContainer factoryContainer, FlashcardSet? flashcardSet = null)
    {
        InitializeComponent();
        InitializeCustomizationFields(flashcardOptionsReference, factoryContainer, flashcardSet);
        CheckIfEditingOrNew(flashcardSet);
    }

    private void InitializeCustomizationFields(FlashcardOptions flashcardOptionsReference, IFactoryContainer factoryContainer, FlashcardSet? flashcardSet = null)
    {
        this.flashcardOptionsReference = flashcardOptionsReference;
        this.factoryContainer = factoryContainer;
        this.flashcardSet = flashcardSet ?? factoryContainer.CreateObject<FlashcardSet>();
        DataContext = this.flashcardSet;
    }

    private async void CheckIfEditingOrNew(FlashcardSet? flashcardSet)
    {
        if (flashcardSet == null)
        {
            IsQestionOrAnswer(false, false);
        }
        else
        {
            await RemoveFromDatabase(flashcardSet);
            RemoveFromFlashcardSetList(flashcardSet);
            ListBoxFlashcards.SelectedIndex = flashcardSet.Flashcards!.Count - 1;
            NameOfSet = flashcardSet.FlashcardSetName;
        }
    }

    private async Task RemoveFromDatabase(FlashcardSet flashcardSet)
    {
        await DatabaseRepository.RemoveFlashcardSetAsync(flashcardSet.FlashcardSetName);
    }

    private void RemoveFromFlashcardSetList(FlashcardSet flashcardSet)
    {
        flashcardOptionsReference.flashcardSets.Remove(flashcardSet);
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
            QuestionTextBox.Focus();
            AnswerBorder.Visibility = Visibility.Collapsed;
            AnswerRadioButton.Visibility = Visibility.Visible;
            ColorBox.Visibility= Visibility.Visible;
            timerListBox.Visibility = Visibility.Visible;
        }
        else
        {
            QuestionBorder.Visibility = Visibility.Collapsed;
            AnswerBorder.Visibility = Visibility.Visible;
            QuestionTextBox.IsEnabled = false;
            AnswerTextBox.IsEnabled = true;
            AnswerTextBox.Focus();
        }
    }

    private void AddFlashcard_Click(object sender, RoutedEventArgs e)
    {
        int index = AddFlashcard();
        ListBoxFlashcards.Items.Refresh();
        ListBoxFlashcards.SelectedIndex = index;
        IsQestionOrAnswer(true, false);
    }

    private int AddFlashcard()
    {
        var flashcard = factoryContainer.CreateObject<Flashcard>();
        int flashcardNumber = flashcardSet.Flashcards!.Count + 1;
        flashcard.FlashcardName = flashcardNumber.ToString("D2");
        flashcardSet.Flashcards.Add(flashcard);
        return flashcardSet.Flashcards.IndexOf(flashcard);
    }

    private void ListBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       IsQestionOrAnswer(true, false);
    }

    private void DeleteFlashcard_Click(object sender, RoutedEventArgs e)
    {
        int oldIndex = DeleteFlashcard(ListBoxFlashcards.SelectedIndex);
        ListBoxFlashcards.Items.Refresh();
        ListBoxFlashcards.SelectedIndex = (oldIndex - 1 < 0) ? 0 : oldIndex - 1;
    }

    private int DeleteFlashcard(int index)
    {
        if(flashcardSet.Flashcards!.Count > 1)
        {
            flashcardSet.Flashcards!.Remove(flashcardSet.Flashcards[index]);
            for (int i = index; i < flashcardSet.Flashcards.Count; i++)
            {
                flashcardSet.Flashcards[i].FlashcardName = (i + 1).ToString("D2");
            }
            return index;
        }
        return index;
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
            SaveFlashcardSetName(NameOfSet.Capitalize());
        }
        else
        {
            FlashcardSetNameBox.Text = NameOfSet;
            SaveFlashcardSetName(NameOfSet!);
        }
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        CapitalizeButton.IsChecked = false;
        NormalizeButton.IsChecked = true;
        CapitalizedNormalNameButton_Click();
    }

    private void SaveFlashcardSetName(string name)
    {
        flashcardSet.FlashcardSetName = name;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        NameOfSet = FlashcardSetNameBox.Text;
    }

    private void DeleteFlashcardSet_Click(object sender, RoutedEventArgs e)
    {
        ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
    }

    private async void SaveFlashcardSet_Click(object sender, RoutedEventArgs e)
    {
        if (IsFlashcardSetCorrect())
        {
            await SaveToDatabase(flashcardSet);
            AddToFlashcardSetsList(flashcardSet);
            ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
        }
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

    private async Task SaveToDatabase(FlashcardSet flashcardSet)
    {
        FlashcardSetDTO flashcardSetDTO = new FlashcardSetDTO();
        flashcardSetDTO.FlashcardSetName = flashcardSet.FlashcardSetName;

        flashcardSetDTO.Flashcards = new ObservableCollection<FlashcardDTO>(
                flashcardSet.Flashcards!.Select(flashcardSet => new FlashcardDTO
                {
                    FlashcardName = flashcardSet.FlashcardName,
                    FlashcardQuestion = flashcardSet.FlashcardQuestion,
                    FlashcardAnswer = flashcardSet.FlashcardAnswer,
                    FlashcardColor = flashcardSet.FlashcardColor,
                    FlashcardTimer = flashcardSet.FlashcardTimer
                }));

        await DatabaseRepository.AddAsync(flashcardSetDTO);
    }

    private void AddToFlashcardSetsList(FlashcardSet flashcardSet)
    {
        flashcardOptionsReference.flashcardSets.Add(flashcardSet);
    }

    private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void TimerListBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }
}
