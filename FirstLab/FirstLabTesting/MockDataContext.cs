using FirstLab;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstLab.src.models;

namespace FirstLabTesting
{
    public class MockDataContext : DbContext
    {
        public MockDataContext(DbContextOptions<MockDataContext> options) : base(options)
        { 
        }

        public DbSet<FlashcardSet> FlashcardSets { get; set; }

        public DbSet<Flashcard> Flashcards { get; set; }

        public DbSet<FlashcardSetLog> FlashcardsLog { get; set; }
    }
}
