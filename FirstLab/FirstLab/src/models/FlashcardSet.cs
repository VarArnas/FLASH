using System;
using System.Collections.ObjectModel;

namespace FirstLab.src.models;

public class FlashcardSet : IEquatable<FlashcardSet>
{
    public string FlashcardSetName { get; set; }

    public ObservableCollection<Flashcard>? Flashcards { get; set; } = new ObservableCollection<Flashcard>();

    public bool Equals(FlashcardSet? other)
    {
        if (other is null)
            return false;
        return FlashcardSetName.Equals(other.FlashcardSetName);
    }
}
