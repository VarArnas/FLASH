

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FirstLab.src.back_end;

public class FlashcardSetDTO
{
    [Key]
    public string FlashcardSetName { get; set; }

    public ObservableCollection<FlashcardDTO>? Flashcards { get; set; } = new ObservableCollection<FlashcardDTO>();
}
