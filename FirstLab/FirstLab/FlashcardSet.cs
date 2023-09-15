using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLab
{
    public class FlashcardSet
    {
        public string SetName { get; set; }

        public ObservableCollection<Flashcard> Flashcards { get; set; } = new ObservableCollection<Flashcard>(); //create a list of flashcards for the set
    }
}
