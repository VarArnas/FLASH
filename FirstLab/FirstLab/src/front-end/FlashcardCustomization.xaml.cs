﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using FirstLab.src.back_end.utilities;

namespace FirstLab
{
    public partial class FlashcardCustomization : UserControl
    {
        private FlashcardOptions flashcardOptionsView;

        public FlashcardSet flashcardSet;

        public Flashcard flashcard;

        //private List<Flashcard> flashcards = new List<Flashcard>();

        //private ObservableCollection<FlashcardSet> flashcardSets;

        private MenuWindow menuWindowReference;

        private String NameOfSet = "Name...";
        public FlashcardCustomization(MenuWindow menuWindowReference, FlashcardSet flashcardSet = null)
        {
            InitializeComponent();

            this.menuWindowReference = menuWindowReference;

            this.flashcardSet = flashcardSet ?? new FlashcardSet();

            this.flashcard = flashcard ?? new Flashcard();

            DataContext = this.flashcardSet;


            QuestionTextBox.IsEnabled = false;
            AnswerTextBox.IsEnabled = false;
            QuestionBorder.Visibility = Visibility.Collapsed;
            AnswerBorder.Visibility = Visibility.Collapsed;

            QuestionRadioButton.Visibility = Visibility.Collapsed;
            AnswerRadioButton.Visibility = Visibility.Collapsed;
        }

        private void AddFlashcard_Click(object sender, RoutedEventArgs e)
        {

            var newFlashcard = new Flashcard();
            int newFlashcardNumber = flashcardSet.Flashcards.Count + 1;
            newFlashcard.FlashcardName = "#" + newFlashcardNumber.ToString();
            flashcardSet.Flashcards.Add(newFlashcard);
            ListBoxFlashcards.Items.Refresh();
            ListBoxFlashcards.SelectedIndex = flashcardSet.Flashcards.IndexOf(newFlashcard);

            QuestionBorder.Visibility = Visibility.Visible;
            QuestionRadioButton.Visibility = Visibility.Visible;
            QuestionRadioButton.IsChecked = true;
            QuestionTextBox.Visibility = Visibility.Visible;
            QuestionTextBox.IsEnabled = true;
            QuestionTextBox.Focus();
            AnswerBorder.Visibility = Visibility.Collapsed;

            AnswerRadioButton.Visibility = Visibility.Visible;

        }

        private void ListBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxFlashcards.SelectedItem != null)
            {
                QuestionBorder.Visibility = Visibility.Visible;
                AnswerBorder.Visibility = Visibility.Collapsed;
                QuestionRadioButton.IsChecked = true;
                QuestionTextBox.IsEnabled = true;
            }
        }

        private void DeleteFlashcard_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBoxFlashcards.SelectedIndex;
            flashcardSet.Flashcards.Remove((Flashcard)ListBoxFlashcards.SelectedItem);
            ListBoxFlashcards.Items.Refresh();

            for (int i = selectedIndex; i < flashcardSet.Flashcards.Count; i++)
            {
                flashcardSet.Flashcards[i].FlashcardName = "#" + (i + 1);
            }
        }

        private void QuestionAnswerRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionRadioButton.IsChecked == true)
            {
                QuestionBorder.Visibility = Visibility.Visible;
                AnswerBorder.Visibility = Visibility.Collapsed;
                QuestionTextBox.IsEnabled = true;
                AnswerTextBox.IsEnabled = false;
            }
            else
            {
                QuestionBorder.Visibility = Visibility.Collapsed;
                AnswerBorder.Visibility = Visibility.Visible;
                QuestionTextBox.IsEnabled = false;
                AnswerTextBox.IsEnabled = true;
            }
        }

        private void QuestionBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(QuestionRadioButton.IsChecked == true)
            {
                QuestionTextBox.Focus();
            }
            else 
            {
                AnswerTextBox.Focus();
            }
        }

        private void CapitalizedNormalNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (CapitalizeButton.IsChecked == true)
            {
                NameOfSet = FlashcardSetNameBox.Text;
                FlashcardSetNameBox.Text = NameOfSet.Capitalize();
            }
            else
            {
                FlashcardSetNameBox.Text = NameOfSet;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CapitalizeButton.IsChecked = false;
            NormalizeButton.IsChecked = true;
            CapitalizedNormalNameButton_Click(NormalizeButton, new RoutedEventArgs(ButtonBase.ClickEvent));

            ControllerUtils.setEmptyText(FlashcardSetNameBox, "Name...");
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            ControllerUtils.setDefaultText(FlashcardSetNameBox, "Name...");
        }

        private void DeleteFlashcardSet_Click(object sender, RoutedEventArgs e)
        {
            flashcardOptionsView = new FlashcardOptions(menuWindowReference.flashcardSets, menuWindowReference);

            menuWindowReference.UpdateHeaderText("Flashcards");

            if (menuWindowReference != null)
            {
                menuWindowReference.contentControl.Content = flashcardOptionsView;
            }
        }

        private void SaveFlashcardSet_Click(object sender, RoutedEventArgs e)
        {
            ListBox listBox = ListBoxFlashcards;

            // Create a copy of the ObservableCollection to avoid the InvalidOperationException
            var flashcardsCopy = new ObservableCollection<Flashcard>(flashcardSet.Flashcards.ToList());

            foreach (var item in listBox.Items)
            {
                if (item is Flashcard flashcardItem)
                {
                    string NameString = flashcardItem.FlashcardName;
                    string QuestionString = flashcardItem.FlashcardQuestion;
                    string AnswerString = flashcardItem.FlashcardAnswer;

                    // Create a new Flashcard for each item in the ListBox
                    Flashcard newFlashcard = new Flashcard
                    {
                        FlashcardName = NameString,
                        FlashcardQuestion = QuestionString,
                        FlashcardAnswer = AnswerString
                    };

                    MessageBox.Show($"Name: {NameString}, Question: {QuestionString}, Answer: {AnswerString}");

                    // Add the new Flashcard to the copy of the ObservableCollection
                    flashcardsCopy.Add(newFlashcard);
                }
            }

            // Update the original ObservableCollection with the modified copy
            flashcardSet.Flashcards = flashcardsCopy;
        }
    }
}
