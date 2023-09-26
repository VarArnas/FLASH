using System.Collections.ObjectModel;

namespace FirstLab
{
    public class FlashcardSet
    {
        public string? FlashcardSetName { get; set; }

        public ObservableCollection<Flashcard> Flashcards { get; set; } = new ObservableCollection<Flashcard>();
    }
}
