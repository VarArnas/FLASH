using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.utilities;
using System.Linq;

namespace FirstLab.src.controllers;

public partial class FlashcardCustomization : UserControl
{
    private FlashcardSet flashcardSet;

    private FlashcardOptions flashcardOptionsReference;

    private string? NameOfSet;

    IFlashcardCustomizationService _flashcardCustomizationService;

    public FlashcardCustomization(FlashcardOptions flashcardOptionsReference, IFactoryContainer factoryContainer,
        IFlashcardCustomizationService flashcardCustomizationService, FlashcardSet? flashcardSet = null)
    {
        InitializeComponent();
        InitializeCustomizationFields(flashcardOptionsReference, factoryContainer, flashcardCustomizationService, flashcardSet);
        CheckIfEditingOrNew(flashcardSet);
    }

    private void FlashcardCustmization_Loaded(object? sender = null, RoutedEventArgs? e = null)
    {
        QuestionTextBox.Focus();
    }

    private void InitializeCustomizationFields(FlashcardOptions flashcardOptionsReference, IFactoryContainer factoryContainer,
        IFlashcardCustomizationService flashcardCustomizationService, FlashcardSet? flashcardSet = null)
    {
        _flashcardCustomizationService = flashcardCustomizationService;
        this.flashcardOptionsReference = flashcardOptionsReference;
        this.flashcardSet = flashcardSet ?? factoryContainer.CreateObject<FlashcardSet>();
        DataContext = this.flashcardSet;
        PreviewKeyDown += UserControl_PreviewKeyDown;
    }

    private void CheckIfEditingOrNew(FlashcardSet? flashcardSet)
    {
        if (!_flashcardCustomizationService.CheckIfEditingAndRemoveTheOldFlashcardSet(flashcardSet, flashcardOptionsReference, NameOfSet))
            AddFlashcard_Click();
        else
        {
            ListBoxFlashcards.SelectedIndex = flashcardSet!.Flashcards!.Count - 1;
            NameOfSet = flashcardSet.FlashcardSetName;
        }
    }

    private void AddFlashcard_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        int index = _flashcardCustomizationService.AddFlashcard(flashcardSet);
        ListBoxFlashcards.Items.Refresh();
        ListBoxFlashcards.SelectedIndex = index;
        IsQestionOrAnswer(true, false);
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (!(e.OriginalSource is TextBox))
            FocusManager.SetFocusedElement(this, this);
    }

    private void ListBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       IsQestionOrAnswer(true, false);
    }

    private void DeleteFlashcard_Click(object sender, RoutedEventArgs e)    
    {
        int oldIndex = _flashcardCustomizationService.DeleteFlashcard(ListBoxFlashcards.SelectedIndex, flashcardSet);
        ListBoxFlashcards.Items.Refresh();
        ListBoxFlashcards.SelectedIndex = _flashcardCustomizationService.CalculateSelectionIndexAfterDeletion(oldIndex);
    }

    private void QuestionAnswerRadioButton_Click(object sender, RoutedEventArgs e)
    {
        IsQestionOrAnswer((bool)QuestionRadioButton.IsChecked!, (bool)AnswerRadioButton.IsChecked!);
    }

    private void CapitalizedNormalNameButton_Click(object? sender = null, RoutedEventArgs? e = null)
    {
       FlashcardSetNameBox.Text = _flashcardCustomizationService.CapitalizeFlashcardSetName(CapitalizeButton.IsChecked, NameOfSet!, FlashcardSetNameBox.Text);
        _flashcardCustomizationService.SaveFlashcardSetName(FlashcardSetNameBox.Text!, flashcardSet);
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
        await _flashcardCustomizationService.CheckErrorsAndSaveFlashcard(flashcardSet, FlashcardSetNameBox.Text, errorText,
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
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            switch (e.Key)
            {
                case Key.OemComma:
                    IsQestionOrAnswer(true, false);
                    break;
                case Key.OemPeriod:
                    IsQestionOrAnswer(false, true);
                    break;
                case Key.Enter:
                    SaveFlashcardSet_Click();
                    break;
            }
        }
        else
        {
            switch (e.Key)
            {
                case Key.Up:
                    NavigateFlashcards(-1);
                    break;
                case Key.Down:
                    NavigateFlashcards(1);
                    break;
                case Key.Escape:
                    ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsReference);
                    break;
            }
        }
    }

    private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBoxItem listBoxItem)
        {
            listBoxItem.IsSelected = !listBoxItem.IsSelected;
            e.Handled = true;
        }
    }

    private void NavigateFlashcards(int direction)
    {
        ListBoxFlashcards.SelectedIndex = _flashcardCustomizationService.CanYouChangeFlashcards(ListBoxFlashcards.SelectedIndex, flashcardSet, direction);
        IsQestionOrAnswer(true, false);
    }

    private void IsQestionOrAnswer(bool question, bool answer)
    {
        QuestionAnswerPropertiesForUI model = _flashcardCustomizationService.ChangeQuestionAnswerProperties(question, answer);
        QuestionBorder.Visibility = model._QuestionBorderVisibility;
        AnswerBorder.Visibility = model._AnswerBorderVisibility;
        QuestionRadioButton.IsChecked = model._CheckQuestionRadioButton;
        AnswerRadioButton.IsChecked = model._CheckAnswerRadioButton;
        (question ? QuestionTextBox : AnswerTextBox).Focus();
    }


    private async void Import_Click(object sender, RoutedEventArgs e)
    {
        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Excel Files|*.xls;*.xlsx"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var flashcardSet = await _flashcardCustomizationService.ReadExcelFile(openFileDialog.FileName);
            if (flashcardSet == null)
            {
                MessageBox.Show("The provided excel structure for the flashcardSet is incorrect");
                return;
            }

            this.flashcardSet = flashcardSet;
            DataContext = this.flashcardSet;
            NameOfSet = flashcardSet.FlashcardSetName;
            if (flashcardSet.Flashcards!.Count() > 0)
            {
                ListBoxFlashcards.SelectedIndex = this.flashcardSet!.Flashcards!.Count - 1;
            }
        }
    }

    private async void Export_Click(object sender, RoutedEventArgs e)
    {
        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "Excel Files|*.xlsx",
            DefaultExt = "xlsx",
            FileName = "ExportedFlashcards"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            await _flashcardCustomizationService.SaveExcelFile(saveFileDialog.FileName, flashcardSet);
        }
    }
}
