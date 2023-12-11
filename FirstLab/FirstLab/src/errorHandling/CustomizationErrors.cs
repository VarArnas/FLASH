﻿using FirstLab.src.models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FirstLab.src.errorHandling;

public class CustomizationErrors
{

    private TextBox errorTextBox;

    private string? nameOfFlashcardSet;

    private FlashcardSet flashcardSet;

    private ObservableCollection<FlashcardSet> SetsOfFlashcards;

    public List<ErrorCode> ErrorCodes { get; private set; }

    public CustomizationErrors(string? nameOfFlashcardSet, FlashcardSet flashcardSet, ObservableCollection<FlashcardSet> SetsOfFlashcards, TextBox? errorTextBox = null) 
    {
        this.errorTextBox = errorTextBox;
        this.nameOfFlashcardSet = nameOfFlashcardSet;
        this.flashcardSet = flashcardSet;
        this.SetsOfFlashcards = SetsOfFlashcards;
        ErrorCodes = new List<ErrorCode>();
    }

    public void CheckForErrors()
    {
        ErrorCodes.Clear();

        if (string.IsNullOrWhiteSpace(nameOfFlashcardSet))
        {
            ErrorCodes.Add(ErrorCode.NameIsEmpty);
        }

        if (nameOfFlashcardSet != null && Regex.IsMatch(nameOfFlashcardSet, @"[^\w\s]+"))
        {
            ErrorCodes.Add(ErrorCode.NotAllowedSymbolsInName);
        }

        if (SetsOfFlashcards.Contains(flashcardSet))
        {
            ErrorCodes.Add(ErrorCode.ExistingName);
        }

        if (!flashcardSet.Flashcards.Any())
        {
            ErrorCodes.Add(ErrorCode.NoFlashcardsExist);
        }

        if (ErrorUtils.AreThereEmptyFlashcards(flashcardSet.Flashcards))
        {
            ErrorCodes.Add(ErrorCode.NotAllFlashcardsFull);
        }
    }

    private void DisplayErrors()
    {
        errorTextBox.Clear();

        foreach (ErrorCode errorCode in ErrorCodes)
        {
            switch (errorCode)
            {
                case ErrorCode.NameIsEmpty:
                    errorTextBox.AppendText("Error: Name is empty!\n\n");
                    break;
                case ErrorCode.NotAllowedSymbolsInName:
                    errorTextBox.AppendText("Error: Name contains symbols!\n\n");
                    break;
                case ErrorCode.ExistingName:
                    errorTextBox.AppendText("Error: Name already exists!\n\n");
                    break;
                case ErrorCode.NoFlashcardsExist:
                    errorTextBox.AppendText("Error: No flashcards exist!\n\n");
                    break;
                case ErrorCode.NotAllFlashcardsFull:
                    errorTextBox.AppendText("Error: Not all flashcards have questions and answers!\n\n");
                    break;
            }
        }
    }

    public void CheckAndDisplayErrors()
    {
        CheckForErrors();
        DisplayErrors();
    }
}
