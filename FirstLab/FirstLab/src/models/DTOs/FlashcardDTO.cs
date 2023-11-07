using System.ComponentModel.DataAnnotations;

namespace FirstLab.src.models.DTOs;

public class FlashcardDTO
{
    [Key]
    public long FlashcardId { get; set; }
    public string FlashcardName { get; set; }
    public string? FlashcardQuestion { get; set; }
    public string? FlashcardAnswer { get; set; }
    public string? FlashcardColor { get; set; }
    public string? FlashcardTimer { get; set; }
}
