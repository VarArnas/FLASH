﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Ink;

namespace FirstLab.src.back_end.errorHandling
{
    public static class ErrorUtils
    {
        public static bool AreThereEmptyFlashcards(ObservableCollection<Flashcard> flashcardSet)
        {
            if (!flashcardSet.Any())
            {
                return false;
            }

            foreach (Flashcard flashcard in flashcardSet)
            {
                if (IsFlashcardEmpty(flashcard))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsFlashcardEmpty(Flashcard flashcard)
        {
            if (string.IsNullOrWhiteSpace(flashcard.FlashcardQuestion) || string.IsNullOrWhiteSpace(flashcard.FlashcardAnswer))
            {
                return true;
            }
            return false;
        }
    }
}
