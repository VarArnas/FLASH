using FirstLab.src.exceptions;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    public int CheckIfPreviousOrNext(bool isPreviousFlashcardNeeded, int index, FlashcardSet flashcardSet, bool isStart)
    {
        if (isPreviousFlashcardNeeded)
            if (!IsFirstOrZeroIndex(index))
                return --index;
            else
                return index;
        else
            if (!IsLastIndex(index, flashcardSet) && !isStart)
                return ++index;
            else
                return index;
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

    public TextAndBorderPropertiesPlayWindow SetQuestionOrAnswerProperties(bool question, bool answer, Flashcard flashcard, FlashcardSet flashcardSet)
    {
        bool isQuestion = DetermineQuestionOrAnswer(question, answer);
        int flashcardIndex = GetFlashcardIndex(flashcard, flashcardSet);
        string flashcardNumber = $"{flashcardIndex + 1}/{flashcardSet.Flashcards!.Count}";
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
        return CreateTextAndBorderProperties(flashcardNumber, text, borderColor!, questionVisibility, answerVisibility);
    }

    public bool DetermineQuestionOrAnswer(bool question, bool answer)
    {
        return question && !answer;
    }

    public int GetFlashcardIndex(Flashcard flashcard, FlashcardSet flashcardSet)
    {
        return flashcardSet.Flashcards.IndexOf(flashcardSet.Flashcards.FirstOrDefault(fc => fc.FlashcardName == flashcard.FlashcardName));
    }

    public TextAndBorderPropertiesPlayWindow CreateTextAndBorderProperties(string flashcardNumber, string text, SolidColorBrush borderColor, Visibility questionVisibility, Visibility answerVisibility)
    {
        return _factoryContainer.CreateTextAndBorderPropertiesPlayWindow(flashcardNumber, text, borderColor, questionVisibility, answerVisibility);
    }


    public int FindCounter(Flashcard flashcard)
    {
        string? selectedTime = null;
        try
        {
            selectedTime = flashcard.FlashcardTimer!.ToString();
        }
        catch (Exception ex)
        {
            ThrowCustomException($"No default timer has been selected", ex);
        }

        return ParseCounter(selectedTime!);
    }

    public int ParseCounter(string selectedTime)
    {
        int counter = 0;
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

    public void CreateCounter(ref int counter, Flashcard flashcard)
    {
        try
        {
            counter = FindCounter(flashcard);
        }
        catch (CustomNullException ex)
        {
            HandleNullTimer(ex);
            counter = 0;
        }
    }

    public TextAndBorderPropertiesPlayWindow GetQuestionAnswerProperties(bool question, bool answer, Flashcard flashcard, FlashcardSet flashcardSet)
    {
        try
        {
            var properties = SetQuestionOrAnswerProperties(question, answer, flashcard, flashcardSet);
            return properties;
        }
        catch (CustomNullException ex)
        {
            HandleNullColor(ex, flashcard);
            var properties = SetQuestionOrAnswerProperties(question, answer, flashcard, flashcardSet);
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

    public void HandleNullColor(CustomNullException ex, Flashcard flashcard)
    {
        CustomNullException.LogException(ex);
        flashcard.FlashcardColor = ex.defaultColor;
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
        if (presentFontSize + sizeChange <= 5 && sizeChange < 0)
            return presentFontSize;
        else if (presentFontSize > 50 && sizeChange > 0)
            return presentFontSize;
        return presentFontSize += sizeChange; 
    }

    public string SetSlidePanelAnimation()
    {
        isPanelVisible = !isPanelVisible;
        return isPanelVisible ? "SlideInAnimation" : "SlideOutAnimation";
    }

    public bool IsLastIndex(int index, FlashcardSet flashcardSet)
    {
        return !(index != flashcardSet.Flashcards!.Count() - 1);
    }

    public bool IsFirstOrZeroIndex(int index)
    {
        return !(index != 0);
    }

    public string CreateQuery(Flashcard currentFlashcard, string userAnswer)
    {
        return new string("You are a teacher evaluating a student's answer. Question: " + currentFlashcard.FlashcardQuestion + ". Correct answer: " + currentFlashcard.FlashcardAnswer + ". Student's answer: " + userAnswer + ". Evaluate the correctness based on content, ensuring all criteria specified in the question are met (the criteria will start after the word \"required\" after the question itself). Score '1' if the student's answer fully meets all criteria. Score '0.5' if it's partially correct or lacks some required information. Score '0' if incorrect. Note: Consider synonyms or equivalent terms as correct, but only if they meet the question's criteria. Provide your response as a single number: '1', '0.5', or '0', and with no additional text or symbols.");
    }

    public async Task<string> CallOpenAIController(string query)
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = "https://localhost:7124/api/OpenAI/UseChatGPT";

                string fullUrl = $"{apiUrl}?query={query}";

                //MessageBox.Show(fullUrl);

                var response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return "Error communicating with the API";
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return "An error occurred";
        }
    }

    public double ExtractNumber(string result)
    {
        Match match = Regex.Match(result, @"-?\d+(\.\d+)?");

        double firstNumber = 5;
        if (match.Success)
        {
            firstNumber = double.Parse(match.Value, CultureInfo.InvariantCulture);
        }

        return firstNumber;
    }

    public SolidColorBrush? GetAnswerColor(double result)
    {
        switch (result)
        {
            case 1:
                return new SolidColorBrush(Colors.Green);
            case 0.5:
                return new SolidColorBrush(Colors.Yellow);
            case 0:
                return new SolidColorBrush(Colors.Red);
            default:
                return null;
        }
    }
}
