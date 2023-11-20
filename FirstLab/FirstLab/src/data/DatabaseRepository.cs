using FirstLab.src.interfaces;
using FirstLab.src.models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.data;

public class DatabaseRepository : IDatabaseRepository
{
    private DataContext _dbContext;

    public DatabaseRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync<T>(T entity) where T : class
    {
        var db = _dbContext;
        db.Set<T>().Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task RemoveAsync<T>(T entity) where T : class
    {
        var db = _dbContext;
        db.Set<T>().Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task<ObservableCollection<T>> GetAllAsync<T>() where T : class
    {
        var db = _dbContext;
        var entities = await db.Set<T>().ToListAsync();
        var collection = new ObservableCollection<T>(entities);
        return collection;
    }

    public async Task RemoveAllAsync<T>() where T : class
    {
        var db = _dbContext;
        var dbSet = db.Set<T>();
        dbSet.RemoveRange(dbSet);
        await db.SaveChangesAsync();
    }

    public async Task RemoveFlashcardSetAsync(string flashcardSetName)
    {
        var db = _dbContext;
        var flashcardSetWithFlashcards = await db.FlashcardSets
            .Include(fs => fs.Flashcards)
            .FirstOrDefaultAsync(fs => fs.FlashcardSetName == flashcardSetName);
        if (flashcardSetWithFlashcards != null)
        {
            db.Flashcards.RemoveRange(flashcardSetWithFlashcards.Flashcards!);
            db.FlashcardSets.Remove(flashcardSetWithFlashcards);
            await db.SaveChangesAsync();
        }
    }

    public async Task<ObservableCollection<FlashcardSetDTO>> GetAllFlashcardSetsAsync()
    {
        var db = _dbContext;
        var flashcardSets = await db.FlashcardSets
                .Include(fs => fs.Flashcards)
                .ToListAsync();
        var collection = new ObservableCollection<FlashcardSetDTO>(flashcardSets);
        return collection;
    }
}
