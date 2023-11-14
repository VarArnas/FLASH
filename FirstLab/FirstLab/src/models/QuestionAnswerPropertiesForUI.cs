using System.Windows;

namespace FirstLab.src.models;

public record QuestionAnswerPropertiesForUI
{
    public Visibility _QuestionBorderVisibility { get; init; }

    public Visibility _AnswerBorderVisibility { get; init; }

    public bool _CheckQuestionRadioButton {  get; init; }

    public bool _CheckAnswerRadioButton { get; init;}

    public QuestionAnswerPropertiesForUI(Visibility QuestionBorderVisibility, Visibility AnswerBorderVisibility, bool CheckQuestionRadioButton, bool CheckAnswerRadioButton)
    {
        _QuestionBorderVisibility = QuestionBorderVisibility;
        _AnswerBorderVisibility = AnswerBorderVisibility;
        _CheckQuestionRadioButton = CheckQuestionRadioButton;
        _CheckAnswerRadioButton = CheckAnswerRadioButton;
    }
}
