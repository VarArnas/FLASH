using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FirstLab.src.models.DTOs;

public class FlashcardSetDTO
{
    [Key]
    public string FlashcardSetName { get; set; }

    public ObservableCollection<FlashcardDTO>? Flashcards { get; set; } = new ObservableCollection<FlashcardDTO>();
}
