

using System.Windows;

namespace FirstLab.src.models;

public class TextModificationProperties
{
    public bool HighlightBtn { get; init; }

    public bool ItalicBtn { get; init; }

    public FontWeight QuestionAnswerTextWeight { get; init; }

    public FontStyle QuestionAnswerTextStyle { get; init; }

    public TextModificationProperties(bool HighlightBtn, bool ItalicBtn, FontWeight QuestionAnswerTextWeight, FontStyle QuestionAnswerTextStyle)
    {
        this.HighlightBtn = HighlightBtn;
        this.ItalicBtn = ItalicBtn;
        this.QuestionAnswerTextWeight = QuestionAnswerTextWeight;
        this.QuestionAnswerTextStyle = QuestionAnswerTextStyle;
    }
}
