using System.Threading.Tasks;
using FirstLab.src.interfaces;
using FirstLab.src.utilities;
using FirstLab.src.data;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using FirstLab.src.errorHandling;
using System.Linq;

namespace FirstLab.src.services;

public class FlashcardCustomizationService : IFlashcardCustomizationService
{
    IFactoryContainer _factoryContainer;

    IFlashcardSetMapper _ifFlashcardSetMapper;

    public FlashcardCustomizationService(IFactoryContainer factoryContainer, IFlashcardSetMapper ifFlashcardSetMapper)
    {
        _factoryContainer = factoryContainer;
        _ifFlashcardSetMapper = ifFlashcardSetMapper;
    }

    public async Task RemoveSetFromDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference)
    {
        flashcardOptionsReference.flashcardSets.Remove(flashcardSet);
        await DatabaseRepository.RemoveFlashcardSetAsync(flashcardSet.FlashcardSetName);
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

        FlashcardSetDTO dto = _ifFlashcardSetMapper.TransformFlashcardSetToDTO(flashcardSet);
        await DatabaseRepository.AddAsync(dto);
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
        if (currentIndex >= 0 && currentIndex < flashcardSet.Flashcards.Count)
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
}
