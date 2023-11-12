using FirstLab;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;

namespace FirstLabTesting
{
    public static class MockDatabaseRepository
    {

        public static async Task AddAsync<T>(T entity, MockDataContext dbContext) where T : class
        {
            var db = dbContext;
            db.Set<T>().Add(entity);
            await db.SaveChangesAsync();
        }

        public static async Task RemoveAsync<T>(T entity, MockDataContext dbContext) where T : class
        {
            var db = dbContext;
            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();
        }

        public static async Task<ObservableCollection<T>> GetAllAsync<T>(MockDataContext dbContext) where T : class
        {
            var db = dbContext;
            var entities = await db.Set<T>().ToListAsync();
            var collection = new ObservableCollection<T>(entities);
            return collection;
        }

        public static async Task RemoveAllAsync<T>(MockDataContext dbContext) where T : class
        {
            var db = dbContext;
            var dbSet = db.Set<T>();
            dbSet.RemoveRange(dbSet);
            await db.SaveChangesAsync();
        }

        public static async Task RemoveFlashcardSetAsync(FlashcardSetDTO flashcardSet, MockDataContext dbContext)
        {
            var db = dbContext;
            var flashcardSetWithFlashcards = await db.FlashcardSets
                .Include(fs => fs.Flashcards)
                .FirstOrDefaultAsync(fs => fs.FlashcardSetName == flashcardSet.FlashcardSetName);
            if (flashcardSetWithFlashcards != null)
            {
                db.Flashcards.RemoveRange(flashcardSetWithFlashcards.Flashcards);
                db.FlashcardSets.Remove(flashcardSetWithFlashcards);
                await db.SaveChangesAsync();
            }
        }

        public static async Task<ObservableCollection<FlashcardSetDTO>> GetAllFlashcardSetsAsync(MockDataContext dbContext)
        {
            var db = dbContext;
            var flashcardSets = await db.FlashcardSets
                    .Include(fs => fs.Flashcards)
                    .ToListAsync();
            var collection = new ObservableCollection<FlashcardSetDTO>(flashcardSets);
            return collection;
        }
    }
}
