using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstLab
{
    public class Flashcard
    {
        [Key]
        [Column(Order = 1)]
        public string FlashcardName { get; set; }

        [Key]
        [Column(Order = 2)]
        public string FlashcardSetName { get; set; }
        public string? FlashcardQuestion { get; set; }
        public string? FlashcardAnswer { get; set; }
        public string? FlashcardColor { get; set; }
    }
}
