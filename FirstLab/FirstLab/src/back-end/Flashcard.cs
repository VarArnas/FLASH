using System.ComponentModel.DataAnnotations;

namespace FirstLab;

public class Flashcard
{
    [Key]
    public string FlashcardId { get; set; }
    public string FlashcardName { get; set; }
    public string? FlashcardQuestion { get; set; }
    public string? FlashcardAnswer { get; set; }
    public string? FlashcardColor { get; set; }
}
