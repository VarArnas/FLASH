using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.back_end.data
{
    public static class DatabaseLibrary
    {
        public static async Task AddFlashcardSetAsync(FlashcardSet flashcardSet)
        {
            using (var context = new DataContext())
            {
                context.FlashcardSets.Add(flashcardSet);
                await context.SaveChangesAsync();
            }
        }

        public static async Task RemoveFlashcardSetAsync(string flashcardSetName)
        {
            using (var context = new DataContext())
            {
                var flashcardSet = await context.FlashcardSets.SingleOrDefaultAsync(f => f.FlashcardSetName == flashcardSetName);
                if (flashcardSet != null)
                {
                    context.FlashcardSets.Remove(flashcardSet);
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task<ObservableCollection<FlashcardSet>> GetAllFlashcardSetsAsync()
        {
            using (var context = new DataContext())
            {
                var flashcardSets = await context.FlashcardSets
                    .Include(fs => fs.Flashcards)
                    .ToListAsync();
                return new ObservableCollection<FlashcardSet>(flashcardSets);
            }
        }

        public static async Task RemoveAllFlashcardSetLogsAsync()
        {
            using (var context = new DataContext())
            {
                await context.Database.ExecuteSqlRawAsync("DELETE FROM FlashcardsLog");
            }
        }

        public static async Task AddFlashcardSetLogAsync(FlashcardSetLog log)
        {
            using (var context = new DataContext())
            {
                context.FlashcardsLog.Add(log);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<ObservableCollection<FlashcardSetLog>> GetAllFlashcardSetLogsAsync()
        {
            using (var context = new DataContext())
            {
                var logs = await context.FlashcardsLog.ToListAsync();
                return new ObservableCollection<FlashcardSetLog>(logs);
            }
        }
    }
}
