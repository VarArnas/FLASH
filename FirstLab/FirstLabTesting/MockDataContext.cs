using FirstLab;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;

namespace FirstLabTesting
{
    public class MockDataContext : DbContext
    {
        public MockDataContext(DbContextOptions<MockDataContext> options) : base(options)
        { 
        }

        public DbSet<FlashcardSetDTO> FlashcardSets { get; set; }

        public DbSet<FlashcardDTO> Flashcards { get; set; }

        public DbSet<FlashcardSetLogDTO> FlashcardsLog { get; set; }
    }
}
