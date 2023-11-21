using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using Microsoft.EntityFrameworkCore;

namespace FirstLab.src.data;

public class DataContext : DbContext
{
    public DbSet<FlashcardSetDTO> FlashcardSets { get; set; }

    public DbSet<FlashcardDTO> Flashcards { get; set; }

    public DbSet<FlashcardSetLogDTO> FlashcardsLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source = C:\Users\Ausra\OneDrive\Documents\GitHub\FirstLab\FirstLab\FirstLab\src\data\myDatabase.db");
}
