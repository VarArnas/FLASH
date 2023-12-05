

using System.Windows;
using System.Windows.Media;

namespace FirstLab.src.models;

public record TextAndBorderPropertiesPlayWindow
{
    public string? FlashcardNumberText { get; init; }

    public string? QuestionAnswerText {  get; init; }
    
    public SolidColorBrush? BorderColor { get; set; }

    public Visibility QuestionBorderVisibility { get; init; }

    public Visibility AnswerBorderVisibility { get; init; }

    public TextAndBorderPropertiesPlayWindow(string FlashcardNumberText, string QuestionAnswerText, SolidColorBrush? BorderColor, Visibility QuestionBorderVisibility, Visibility AnswerBorderVisibility)
    {
        this.FlashcardNumberText = FlashcardNumberText;
        this.QuestionAnswerText = QuestionAnswerText;
        this.BorderColor = BorderColor; 
        this.QuestionBorderVisibility = QuestionBorderVisibility;
        this.AnswerBorderVisibility = AnswerBorderVisibility;
    }
}
