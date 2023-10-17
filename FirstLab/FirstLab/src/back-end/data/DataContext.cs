using Microsoft.EntityFrameworkCore;

namespace FirstLab.src.back_end.data
{
    class DataContext : DbContext
    {
        public DbSet<FlashcardSet> FlashcardSets { get; set; }

        public DbSet<Flashcard> Flashcards { get; set; }

        public DbSet<FlashcardSetLog> FlashcardsLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flashcard>()
                .HasKey(f => new { f.FlashcardName, f.FlashcardSetName });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=C:\\Users\\arnas\\Desktop\\FirstLab\\FirstLab\\FirstLab\\src\\back-end\\data\\myDatabase.db"); //need to change just copy full path for now but later create a constructor with a connection string and make it optional
    }
}
