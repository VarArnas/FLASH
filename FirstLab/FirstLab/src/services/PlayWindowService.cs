using FirstLab.src.exceptions;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FirstLab.src.services;

public class PlayWindowService : IPlayWindowService
{

    IFactoryContainer _factoryContainer;

    private bool isPanelVisible = true;

    public PlayWindowService(IFactoryContainer factoryContainer)
    { 
        _factoryContainer = factoryContainer;
    }

    public DoubleAnimation SetAnimation()
    {
        DoubleAnimation opacityAnimation = new DoubleAnimation();
        opacityAnimation.From = 1.0;
        opacityAnimation.To = 0.1;
        opacityAnimation.Duration = TimeSpan.FromSeconds(2);
        opacityAnimation.AutoReverse = true;
        opacityAnimation.RepeatBehavior = RepeatBehavior.Forever;
        return opacityAnimation;
    }

    public TextAndBorderPropertiesPlayWindow SetQuestionOrAnswerProperties(bool question, bool answer, int currentFlashcardIndex, FlashcardSet flashcardSet)
    {
        bool isQuestion = question && !answer;
        int index = isQuestion ? currentFlashcardIndex : currentFlashcardIndex - 1;
        var flashcard = flashcardSet.Flashcards![index]!;
        string flashcardNumber = $"{index + 1}/{flashcardSet.Flashcards!.Count}";
        string text = isQuestion ? flashcard.FlashcardQuestion! : flashcard.FlashcardAnswer!;
        SolidColorBrush? borderColor = null;

        try
        {
            borderColor = (SolidColorBrush)new BrushConverter().ConvertFromString(flashcard.FlashcardColor!)!;
        }
        catch (Exception ex)
        {
            ThrowCustomException("No default color has been selected", ex);
        }

        Visibility questionVisibility = isQuestion ? Visibility.Visible : Visibility.Collapsed;
        Visibility answerVisibility = isQuestion ? Visibility.Collapsed : Visibility.Visible;
        return _factoryContainer.CreateTextAndBorderPropertiesPlayWindow(flashcardNumber, text, borderColor, questionVisibility, answerVisibility);
    }


    public int FindCounter(int ind, FlashcardSet flashcardSet)
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

    public void CreateCounter(ref int counter, int ind, FlashcardSet flashcardSet)
    {
        try
        {
            counter = FindCounter(ind, flashcardSet);
        }
        catch (CustomNullException ex)
        {
            HandleNullTimer(ex);
            counter = 0;
        }
    }

    public TextAndBorderPropertiesPlayWindow GetQuestionAnswerProperties(bool question, bool answer, int currentFlashcardIndex, FlashcardSet flashcardSet)
    {
        try
        {
            var properties = SetQuestionOrAnswerProperties(true, false, currentFlashcardIndex, flashcardSet);
            return properties;
        }
        catch (CustomNullException ex)
        {
            HandleNullColor(ex, flashcardSet, currentFlashcardIndex);
            var properties = SetQuestionOrAnswerProperties(true, false, currentFlashcardIndex, flashcardSet);
            return properties;
        }
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

    public void TryToIncrementCurrentIndex(ref int currentIndex, FlashcardSet flashcardSet)
    {
        if (!IsIndexOverBounds(currentIndex, flashcardSet))
            currentIndex++;
    }

    public bool IsIndexOverBounds(int index, FlashcardSet flashcardSet)
    {
        return !(index >= 0 && (index < flashcardSet.Flashcards!.Count() || index == 0));
    }

    public void HandleNullColor(CustomNullException ex, FlashcardSet flashcardSet, int flashcardIndex)
    {
        CustomNullException.LogException(ex);
        flashcardSet.Flashcards![flashcardIndex].FlashcardColor = ex.defaultColor;
    }

    public void HandleNullTimer(CustomNullException ex)
    {
        CustomNullException.LogException(ex);
    }

    public TextModificationProperties SetTextProperties(bool isHighlighted, bool isItalic)
    {
        FontWeight fontWeight = isHighlighted ? FontWeights.Bold : FontWeights.Normal;
        FontStyle fontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal;

        return _factoryContainer.CreateTextModificationProperties(isHighlighted, isItalic, fontWeight, fontStyle);
    }

    public double FindNewTextSize(bool increaseSize, FlashcardDesign flashcardDesign, double presentFontSize)
    {
        double sizeChange = increaseSize ? flashcardDesign.IncreaseTextSize : -flashcardDesign.DecreaseTextSize;
        return presentFontSize += sizeChange; 
    }

    public string SetSlidePanelAnimation()
    {
        isPanelVisible = !isPanelVisible;
        return isPanelVisible ? "SlideInAnimation" : "SlideOutAnimation";
    }
}
