using FirstLab.src.back_end.errorHandling;
using FirstLab.src.back_end.factories.factoryInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace FirstLab.src;

public class PlayWindowService : IPlayWindowService
{

    IFactoryContainer _factoryContainer;

    public PlayWindowService(IFactoryContainer factoryContainer)
    { 
        _factoryContainer = factoryContainer;
    }

    public int SetTheCounter(int ind, FlashcardSet flashcardSet)
    {
        string? selectedTime = null;
        int counter = 0;
        try
        {
            selectedTime = flashcardSet.Flashcards![ind].FlashcardTimer!.ToString();
        }
        catch (Exception ex)
        {
            ThrowCustomException($"No default timer has been selected", ex);
        }

        if (!string.IsNullOrEmpty(selectedTime))
        {
            Match match = Regex.Match(selectedTime, @"\d+");

            if (match.Success && int.TryParse(match.Value, out int timerCounter))
            {
                counter = timerCounter;
                return counter;
            }

            return counter;
        }

        return counter;
    }

    public void ShuffleFlashcards(ObservableCollection<Flashcard> flashcards)
    {
        Random random = new Random();

        for (int i = flashcards.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Flashcard temp = flashcards[i];
            flashcards[i] = flashcards[j];
            flashcards[j] = temp;
        }
    }

    public FlashcardSet CloneFlashcardSet(FlashcardSet originalSet)
    {
        FlashcardSet clonedSet = _factoryContainer.CreateObject<FlashcardSet>();
        foreach (var flashcard in originalSet.Flashcards!)
        {
            clonedSet.Flashcards!.Add(flashcard);
        }
        clonedSet.FlashcardSetName = originalSet.FlashcardSetName;
        return clonedSet;
    }
    public void ThrowCustomException(string message, Exception exception)
    {
        CustomNullException.LogException(exception);
        throw _factoryContainer.CreateException(message);
    }

    public void LogCustomException(string message)
    {
        CustomNullException displayError = _factoryContainer.CreateException(message);
        CustomNullException.LogException(displayError);
    }

    public bool IsIndexOverBounds(int index, FlashcardSet flashcardSet)
    {
        return !(index >= 0 && index < flashcardSet.Flashcards!.Count());
    }

    public void HandleNullColor(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex)
    {
        CustomNullException.LogException(ex);
        flashcardSet.Flashcards![flashcardIndex].FlashcardColor = ex.defaultColor;
    }

    public void HandleNullTimer(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex)
    {
        CustomNullException.LogException(ex);
        flashcardSet.Flashcards![flashcardIndex].FlashcardTimer = ex.defaultTime;
    }
}
