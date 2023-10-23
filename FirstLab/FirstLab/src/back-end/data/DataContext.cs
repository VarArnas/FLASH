using FirstLab.src.back_end.utilities;
using Microsoft.EntityFrameworkCore;

namespace FirstLab.src.back_end.data
{
    class DataContext : DbContext
    {
        public DbSet<FlashcardSet> FlashcardSets { get; set; }

        public DbSet<Flashcard> Flashcards { get; set; }

        public DbSet<FlashcardSetLog> FlashcardsLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={TextUtils.ReturnDatabaseString()}");
    }
}
