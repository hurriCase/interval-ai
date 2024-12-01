using System.Collections;
using System.Linq;
using Client.Scripts.DB;
using Client.Scripts.DB.Entities;
using Client.Scripts.Patterns.DI.Base;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Client.Tests.Runtime.EntityControllerTests
{
    internal class EntityControllerTests : Injectable
    {
        [Inject] private IEntityController _entityController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // This ensures the test runs in play mode
            Assert.That(UnityEngine.Application.isPlaying, Is.True,
                "These tests must be run in play mode!");
        }

        [UnityTest]
        public IEnumerator CreateEntity_WithValidData_ShouldSucceed()
        {
            // Arrange
            var userData = new UserEntityData
            {
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var createTask = _entityController.CreateEntityAsync(userData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            var result = createTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.UserName, Is.EqualTo("TestUser"));
        }

        [UnityTest]
        public IEnumerator ReadEntity_AfterCreation_ShouldReturnData()
        {
            // Arrange
            var categoryData = new CategoryEntityData
            {
                Title = "Test Category",
                Description = "Test Description",
                IsDefault = false
            };

            var createTask = _entityController.CreateEntityAsync(categoryData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            // Act
            var readTask = _entityController.ReadEntityAsync<CategoryEntityData>();
            while (!readTask.IsCompleted)
            {
                yield return null;
            }

            var result = readTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Title, Is.EqualTo("Test Category"));
        }

        [UnityTest]
        public IEnumerator UpdateEntity_WithValidData_ShouldSucceed()
        {
            // Arrange
            var wordData = new WordEntityData
            {
                NativeWord = "Hello",
                LearningWord = "Bonjour",
                Transcription = "bɔ̃ʒuʁ"
            };

            var createTask = _entityController.CreateEntityAsync(wordData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            // Act
            wordData.Transcription = "Updated Transcription";
            var updateTask = _entityController.UpdateEntityAsync(wordData);
            while (!updateTask.IsCompleted)
            {
                yield return null;
            }

            var result = updateTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Transcription, Is.EqualTo("Updated Transcription"));
        }

        [UnityTest]
        public IEnumerator DeleteEntity_ExistingEntity_ShouldSucceed()
        {
            // Arrange
            var progressData = new ProgressEntityData
            {
                WordId = "test-word-id",
                RepetitionStage = 1,
                TotalReviews = 5
            };

            var createTask = _entityController.CreateEntityAsync(progressData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            var entityId = createTask.Result.Data.WordId;

            // Act
            var deleteTask = _entityController.DeleteEntityAsync<ProgressEntityData>(entityId);
            while (!deleteTask.IsCompleted)
            {
                yield return null;
            }

            var result = deleteTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator ExistsAsync_WithExistingEntity_ShouldReturnTrue()
        {
            // Arrange
            var userData = new UserEntityData
            {
                UserName = "ExistingUser",
                Email = "existing@example.com"
            };

            var createTask = _entityController.CreateEntityAsync(userData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            // Act
            var existsTask = _entityController.ExistsAsync<UserEntityData>();
            while (!existsTask.IsCompleted)
            {
                yield return null;
            }

            // Assert
            Assert.That(existsTask.Result, Is.True);
        }

        [UnityTest]
        public IEnumerator FindEntities_WithMatchingPredicate_ShouldReturnResults()
        {
            // Arrange
            var word1 = new WordEntityData { NativeWord = "Hello", LearningWord = "Bonjour" };
            var word2 = new WordEntityData { NativeWord = "Goodbye", LearningWord = "Au revoir" };

            var createTask1 = _entityController.CreateEntityAsync(word1);
            var createTask2 = _entityController.CreateEntityAsync(word2);

            while (!createTask1.IsCompleted || !createTask2.IsCompleted)
            {
                yield return null;
            }

            // Act
            var results = _entityController.FindEntitiesAsync<WordEntityData>(
                w => w.LearningWord.Contains("Bonjour")
            );

            // Assert
            var wordEntityDatas = results as WordEntityData[] ?? results.ToArray();
            Assert.That(wordEntityDatas, Is.Not.Null);
            Assert.That(wordEntityDatas.Length, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator EntityEvents_ShouldTriggerAppropriately()
        {
            // Arrange
            var eventTriggered = false;
            _entityController.OnEntityCreated += _ => eventTriggered = true;

            var userData = new UserEntityData
            {
                UserName = "EventTestUser",
                Email = "event@example.com"
            };

            // Act
            var createTask = _entityController.CreateEntityAsync(userData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            // Assert
            Assert.That(eventTriggered, Is.True, "Entity created event should have been triggered");
        }

        [UnityTest]
        public IEnumerator CreateEntity_WithInvalidType_ShouldReturnFailure()
        {
            // Act
            var createTask = _entityController.CreateEntityAsync(new InvalidTestEntity());
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            var result = createTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Entity type not registered"));
        }

        private class InvalidTestEntity
        {
            public string Name { get; set; }
        }
    }
}