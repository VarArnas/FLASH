using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstLab.src.back_end.data
{
    public static class DatabaseRepository
    {
        public static async Task AddAsync<T>(T entity) where T : class
        {
            using (var context = new DataContext())
            {
                context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public static async Task RemoveAsync<T>(T entity) where T : class
        {
            using (var context = new DataContext())
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<ObservableCollection<T>> GetAllAsync<T>() where T : class
        {
            using (var context = new DataContext())
            {
                var entities = await context.Set<T>().ToListAsync();
                return new ObservableCollection<T>(entities);
            }
        }

        public static async Task RemoveAllAsync<T>() where T : class
        {
            using (var context = new DataContext())
            {
                var dbSet = context.Set<T>();
                dbSet.RemoveRange(dbSet);
                await context.SaveChangesAsync();
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
    }
}
