using System.Threading.Tasks;
using FirstLab.src.interfaces;
using FirstLab.src.utilities;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using FirstLab.src.errorHandling;
using System.Linq;
using FirstLab.src.controllers;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System;

namespace FirstLab.src.services;

public class FlashcardCustomizationService : IFlashcardCustomizationService
{
    IFactoryContainer _factoryContainer;

    IFlashcardSetMapper _flashcardSetMapper;

    IDatabaseRepository _databaseRepository;

    public FlashcardCustomizationService(IFactoryContainer factoryContainer, IFlashcardSetMapper flashcardSetMapper, IDatabaseRepository databaseRepository)
    {
        _factoryContainer = factoryContainer;
        _flashcardSetMapper = flashcardSetMapper;
        _databaseRepository = databaseRepository;
    }

    public async Task RemoveSetFromDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference)
    {
        flashcardOptionsReference.flashcardSets.Remove(flashcardSet);
        flashcardOptionsReference.filteredFlashcardSets.Remove(flashcardSet);
        await _databaseRepository.RemoveFlashcardSetAsync(flashcardSet.FlashcardSetName);
    }

    public int AddFlashcard(FlashcardSet flashcardSet)
    {
        var flashcard = _factoryContainer.CreateObject<Flashcard>();
        int flashcardNumber = flashcardSet.Flashcards!.Count + 1;
        flashcard.FlashcardName = flashcardNumber.ToString("D2");
        flashcardSet.Flashcards.Add(flashcard);
        return flashcardSet.Flashcards.IndexOf(flashcard);
    }

    public int DeleteFlashcard(int index, FlashcardSet flashcardSet)
    {
        if (flashcardSet.Flashcards!.Count > 1)
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

    public void SaveFlashcardSetName(string name, FlashcardSet flashcardSet)
    {
        flashcardSet.FlashcardSetName = name;
    }

    public async Task SaveToDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference)
    {
        flashcardOptionsReference.flashcardSets.Add(flashcardSet);
        flashcardOptionsReference.filteredFlashcardSets.Add(flashcardSet);

        FlashcardSetDTO dto = _flashcardSetMapper.TransformFlashcardSetToDTO(flashcardSet);
        await _databaseRepository.AddAsync(dto);
    }

    public QuestionAnswerPropertiesForUI ChangeQuestionAnswerProperties(bool question, bool answer)
    {
        if(question && !answer)
        {
            return _factoryContainer.CreateQuestionAnswerProperties(Visibility.Visible, Visibility.Collapsed, true, false);
        }
        else
        {
            return _factoryContainer.CreateQuestionAnswerProperties(Visibility.Collapsed, Visibility.Visible, false, true);
        }
    }

    public int CalculateSelectionIndexAfterDeletion(int oldIndex)
    {
        return (oldIndex - 1 <0) ? 0 : oldIndex - 1;
    }

    public string CapitalizeFlashcardSetName(bool? isCapitalizationNeeded, string NameOfSet, string flashcardSetNameTextBox)
    {
        if ((bool)isCapitalizationNeeded!)
        {
            NameOfSet = flashcardSetNameTextBox;
            return NameOfSet.Capitalize();
        }
        else
        {
            return NameOfSet;
        }
    }

    public CustomizationErrors InitializeErrors(FlashcardSet flashcardSet, string nameOfFlashcardSet, TextBox errorText, ObservableCollection<FlashcardSet> SetsOfFlashcards)
    {
        return _factoryContainer.CreateErrorHandling(
           flashcardSet: flashcardSet,
           nameOfFlashcardSet: nameOfFlashcardSet,
           errorTextBox: errorText,
           SetsOfFlashcards: SetsOfFlashcards
       );
    }

    public bool IsFlashcardSetCorrect(FlashcardSet flashcardSet, string nameOfFlashcardSet, TextBox errorText, ObservableCollection<FlashcardSet> SetsOfFlashcards)
    {
        CustomizationErrors errors = InitializeErrors(flashcardSet, nameOfFlashcardSet, errorText, SetsOfFlashcards);
        errors.CheckAndDisplayErrors();
        return !errors.ErrorCodes.Any();
    }

    public int CanYouChangeFlashcards(int currentIndex, FlashcardSet flashcardSet, int direction)
    {
        if (currentIndex >= 0 && currentIndex < flashcardSet.Flashcards!.Count)
        {
            int newIndex = currentIndex + direction;
            if (newIndex >= 0 && newIndex < flashcardSet.Flashcards.Count)
            {
                return newIndex;
            }
            return currentIndex;
        }
        return currentIndex;
    }

    public async Task CheckErrorsAndSaveFlashcard(FlashcardSet flashcardSet, string nameOfFlashcardSet, TextBox errorText, ObservableCollection<FlashcardSet> SetsOfFlashcards,
        FlashcardOptions flashcardOptions)
    {
        if (IsFlashcardSetCorrect(flashcardSet, nameOfFlashcardSet, errorText, SetsOfFlashcards))
        {
            await SaveToDatabase(flashcardSet, flashcardOptions);
            ViewsUtils.ChangeWindow("Flashcards", flashcardOptions);
        }
    }

    public bool CheckIfEditingAndRemoveTheOldFlashcardSet(FlashcardSet? flashcardSet, FlashcardOptions flashcardOptionsReference, string? NameOfSet)
    {
        if(flashcardSet == null)
        {
            return false;
        }
        else
        {
            RemoveSetFromDatabase(flashcardSet, flashcardOptionsReference);
            NameOfSet = flashcardSet.FlashcardSetName;
            return true;
        }
    }

    public async Task<FlashcardSet?> ReadExcelFile(string filePath)
    {
        return await Task.Run(() =>
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FlashcardSet flashcardSet = _factoryContainer.CreateObject<FlashcardSet>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                flashcardSet.FlashcardSetName = worksheet.Cells[1, 1].Value?.ToString();

                if (flashcardSet.FlashcardSetName == null)
                {
                    return null;
                }

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var flashcard = _factoryContainer.CreateObject<Flashcard>();
                    flashcard.FlashcardQuestion = worksheet.Cells[row, 1].Value?.ToString();
                    flashcard.FlashcardAnswer = worksheet.Cells[row, 2].Value?.ToString();
                    flashcard.FlashcardColor = IsValidColor(worksheet.Cells[row, 3].Value?.ToString());
                    flashcard.FlashcardTimer = ValidateTimer(worksheet.Cells[row, 4].Value?.ToString());

                    if (flashcard.FlashcardQuestion == null || flashcard.FlashcardAnswer == null)
                    {
                        return null;
                    }

                    flashcard.FlashcardName = (row - 1).ToString("D2");

                    flashcardSet.Flashcards.Add(flashcard);
                }
            }

            return flashcardSet;
        });
    }

    public string? IsValidColor(string? colorName)
    {
        if (string.IsNullOrWhiteSpace(colorName))
        {
            return null;
        }

        var colorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"Red", "IndianRed"},
            {"Blue", "RoyalBlue"},
            {"IndianRed", "IndianRed"},
            {"RoyalBlue", "RoyalBlue"},
            {"Yellow", "Yellow"},
            {"Pink", "Pink"},
            {"Orange", "Orange"}
        };

        if (colorMap.TryGetValue(colorName, out var validColor))
        {
            return validColor;
        }

        return null;
    }

    public string? ValidateTimer(string? timerValue)
    {
        var validTimers = new HashSet<int> { 3, 5, 10, 15, 30 };
        if (int.TryParse(timerValue, out var timer) && validTimers.Contains(timer))
        {
            return timer.ToString();
        }

        return null;
    }

    public async Task SaveExcelFile(string filePath, FlashcardSet flashcardSet)
    {
        await Task.Run(() =>
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Flashcards");
                worksheet.Cells[1, 1].Value = flashcardSet.FlashcardSetName;

                int row = 2;
                foreach (var flashcard in flashcardSet.Flashcards)
                {
                    worksheet.Cells[row, 1].Value = flashcard.FlashcardQuestion;
                    worksheet.Cells[row, 2].Value = flashcard.FlashcardAnswer;
                    worksheet.Cells[row, 3].Value = flashcard.FlashcardColor;
                    worksheet.Cells[row, 4].Value = flashcard.FlashcardTimer;
                    row++;
                }

                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        });
    }
}
