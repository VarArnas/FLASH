using System.Windows;

namespace FirstLab.src.models;

public record PlayWindowAnswer
{
    public string? AnswerText {  get; init; }

    public Visibility QuestionBorder {  get; init; }

    public Visibility AnswerBorder { get; init; }

    public PlayWindowAnswer(string? AnswerText, Visibility QuestionBorder, Visibility AnswerBorder)
    {
        this.AnswerText = AnswerText;
        this.QuestionBorder = QuestionBorder;
        this.AnswerBorder = AnswerBorder;
    }
}
