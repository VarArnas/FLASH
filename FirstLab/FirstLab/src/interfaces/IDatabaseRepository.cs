using FirstLab.src.models.DTOs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.interfaces;

public interface IDatabaseRepository
{
    Task AddAsync<T>(T entity) where T : class;
    Task<ObservableCollection<T>> GetAllAsync<T>() where T : class;
    Task<ObservableCollection<FlashcardSetDTO>> GetAllFlashcardSetsAsync();
    Task RemoveAllAsync<T>() where T : class;
    Task RemoveAsync<T>(T entity) where T : class;
    Task RemoveFlashcardSetAsync(string flashcardSetName);
}