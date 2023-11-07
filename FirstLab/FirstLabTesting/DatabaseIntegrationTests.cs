using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstLab;
using FirstLab.src.models;

namespace FirstLabTesting
{
    [Collection("DatabaseCollection")]
    public class DatabaseIntegrationTests
    {
        private readonly MockDataContext _dbContext;

        public DatabaseIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<MockDataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
                .Options;

            _dbContext = new MockDataContext(options);
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntityToDatabase()
        {
            // Arrange
            var flashcardSet = new FlashcardSet { FlashcardSetName = "Set1"};

            // Act
            await MockDatabaseRepository.AddAsync(flashcardSet, _dbContext);
            var retrievedEntities = await MockDatabaseRepository.GetAllFlashcardSetsAsync(_dbContext);

            // Assert
            Assert.NotEmpty(retrievedEntities);
            Assert.Contains(flashcardSet, retrievedEntities);
         
        }

        [Fact]
        public async void RemoveFlashcardSetAsync_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var flashcardSet = new FlashcardSet { FlashcardSetName = "Set2" };

            // Act
            await MockDatabaseRepository.AddAsync(flashcardSet, _dbContext);
            await MockDatabaseRepository.RemoveFlashcardSetAsync(flashcardSet, _dbContext);
            var retrievedEntities = await MockDatabaseRepository.GetAllFlashcardSetsAsync(_dbContext);

            // Assert
            Assert.DoesNotContain(flashcardSet, retrievedEntities);
        }

        [Fact]
        public async void RemoveAsync_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var flashcard = new Flashcard { FlashcardName = "flashcard"};

            // Act
            await MockDatabaseRepository.AddAsync(flashcard, _dbContext);
            await MockDatabaseRepository.RemoveAsync(flashcard, _dbContext);
            var retrievedEntities = await MockDatabaseRepository.GetAllAsync<Flashcard>(_dbContext);

            // Assert
            Assert.DoesNotContain(flashcard, retrievedEntities);
            Assert.Empty(retrievedEntities);
        }

        [Fact] 
        public async void GetAllAsync_ShouldReturnAllEntitiesOfSpecifiedType()
        {
            // Arrange
            var flashcard1 = new Flashcard { FlashcardName = "flashcard1" };
            var flashcard2 = new Flashcard { FlashcardName = "flashcard2" };
            var flashcard3 = new Flashcard { FlashcardName = "flashcard3" };

            // Act
            await MockDatabaseRepository.AddAsync(flashcard1, _dbContext);
            await MockDatabaseRepository.AddAsync(flashcard2, _dbContext);
            await MockDatabaseRepository.AddAsync(flashcard3, _dbContext);
            var retrievedEntities = await MockDatabaseRepository.GetAllAsync<Flashcard>(_dbContext);

            // Assert
            Assert.NotEmpty(retrievedEntities);
            Assert.Contains(flashcard1, retrievedEntities);
            Assert.Contains(flashcard2, retrievedEntities);
            Assert.Contains(flashcard3, retrievedEntities);

        }

        [Fact]
        public async void RemoveAllAsync_ShouldRemoveAllEntitiesOfSpecifiedType()
        {
            // Arrange
            var flashcard1 = new Flashcard { FlashcardName = "flashcard1" };
            var flashcard2 = new Flashcard { FlashcardName = "flashcard2" };
            var flashcard3 = new Flashcard { FlashcardName = "flashcard3" };

            // Act
            await MockDatabaseRepository.AddAsync(flashcard1, _dbContext);
            await MockDatabaseRepository.AddAsync(flashcard2, _dbContext);
            await MockDatabaseRepository.AddAsync(flashcard3, _dbContext);
            await MockDatabaseRepository.RemoveAllAsync<Flashcard>(_dbContext);
            var retrievedEntities = await MockDatabaseRepository.GetAllAsync<Flashcard>(_dbContext);

            // Assert
            Assert.DoesNotContain(flashcard1, retrievedEntities);
            Assert.DoesNotContain(flashcard2, retrievedEntities);
            Assert.DoesNotContain(flashcard3, retrievedEntities);
            Assert.Empty(retrievedEntities);
        }

        [Fact]
        public async void GetAllFlashcardSetsAsync_ShouldReturnAllFlashcardSets()
        {
            // Arrange
            var flashcardSet1 = new FlashcardSet { FlashcardSetName = "flashcardSet1" };
            var flashcardSet2 = new FlashcardSet { FlashcardSetName = "flashcardSet2" };
            var flashcardSet3 = new FlashcardSet { FlashcardSetName = "flashcardSet3" };

            // Act
            await MockDatabaseRepository.AddAsync(flashcardSet1, _dbContext);
            await MockDatabaseRepository.AddAsync(flashcardSet2, _dbContext);
            await MockDatabaseRepository.AddAsync(flashcardSet3, _dbContext);
            var retrievedEntities = await MockDatabaseRepository.GetAllFlashcardSetsAsync(_dbContext);

            // Assert
            Assert.NotEmpty(retrievedEntities);
            Assert.Contains(flashcardSet1, retrievedEntities);
            Assert.Contains(flashcardSet2, retrievedEntities);
            Assert.Contains(flashcardSet3, retrievedEntities);
        }
    }
}
