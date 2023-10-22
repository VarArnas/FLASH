namespace FirstLab.src.back_end;

public struct FlashcardDesign
{
    public bool IsItalic { get; set; }

    public bool IsHighlighted { get; set; }

    public int IncreaseTextSize { get; set; }

    public int DecreaseTextSize { get; set; }

    public FlashcardDesign(bool isItalic, bool isHighlighted, int increaseTextSize, int decreaseTextSize)
    {
        IsItalic = isItalic;
        IsHighlighted = isHighlighted;
        IncreaseTextSize = increaseTextSize;
        DecreaseTextSize = decreaseTextSize;
    }
}