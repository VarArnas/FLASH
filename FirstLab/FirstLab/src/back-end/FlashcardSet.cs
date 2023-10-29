using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FirstLab
{
    public class FlashcardSet : IEquatable<FlashcardSet>
    {
        [Key]
        public string FlashcardSetName { get; set; }

        public ObservableCollection<Flashcard>? Flashcards { get; set; } = new ObservableCollection<Flashcard>();

        public bool Equals(FlashcardSet? other)
        {
            if (other is null)
                return false;
            return FlashcardSetName.Equals(other.FlashcardSetName);
        }
    }
}
