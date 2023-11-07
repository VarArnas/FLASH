using System.Threading.Tasks;
using FirstLab.src.models;

namespace FirstLab.src.interfaces
{
    public interface IFlashcardCustomizationService
    {
        int AddFlashcard(FlashcardSet flashcardSet);
        int DeleteFlashcard(int index, FlashcardSet flashcardSet);
        Task RemoveSetFromDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference);
        void SaveFlashcardSetName(string name, FlashcardSet flashcardSet);
        Task SaveToDatabase(FlashcardSet flashcardSet, FlashcardOptions flashcardOptionsReference);
    }
}