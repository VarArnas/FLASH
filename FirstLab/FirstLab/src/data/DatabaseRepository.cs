using FirstLab.src.models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.data;

public static class DatabaseRepository
{
    public static IServiceProvider? serviceProvider;

    public static async Task AddAsync<T>(T entity) where T : class
    {
        var db = serviceProvider!.GetRequiredService<DataContext>();
        db.Set<T>().Add(entity);
        await db.SaveChangesAsync();
    }

    public static async Task RemoveAsync<T>(T entity) where T : class
    {
        var db = serviceProvider!.GetRequiredService<DataContext>();
        db.Set<T>().Remove(entity);
        await db.SaveChangesAsync();
    }

    public static async Task<ObservableCollection<T>> GetAllAsync<T>() where T : class
    {
        var db = serviceProvider!.GetRequiredService<DataContext>();
        var entities = await db.Set<T>().ToListAsync();
        var collection = new ObservableCollection<T>(entities);
        return collection;
    }

    public static async Task RemoveAllAsync<T>() where T : class
    {
        var db = serviceProvider!.GetRequiredService<DataContext>();
        var dbSet = db.Set<T>();
        dbSet.RemoveRange(dbSet);
        await db.SaveChangesAsync();
    }

    public static async Task RemoveFlashcardSetAsync(string flashcardSetName)
    {
        var db = serviceProvider!.GetRequiredService<DataContext>();
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

    public static async Task<ObservableCollection<FlashcardSetDTO>> GetAllFlashcardSetsAsync()
    {
        var db = serviceProvider!.GetRequiredService<DataContext>();
        var flashcardSets = await db.FlashcardSets
                .Include(fs => fs.Flashcards)
                .ToListAsync();
        var collection = new ObservableCollection<FlashcardSetDTO>(flashcardSets);
        return collection;
    }
}
