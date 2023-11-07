using FirstLab.src.back_end.utilities;
using Microsoft.EntityFrameworkCore;

namespace FirstLab.src.back_end.data
{
    class DataContext : DbContext
    {
        public DbSet<FlashcardSetDTO> FlashcardSets { get; set; }

        public DbSet<FlashcardDTO> Flashcards { get; set; }

        public DbSet<FlashcardSetLogDTO> FlashcardsLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source = {TextUtils.ReturnDatabaseString}");
    }
}
