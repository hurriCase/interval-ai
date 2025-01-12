using System;
using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Core;
using Client.Scripts.Core.StartUp;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.DB.Entities.ProgressEntity;
using Client.Scripts.DB.Entities.User;
using Client.Scripts.DB.Entities.UserCategory;
using Client.Scripts.DB.Entities.Word;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Client.Tests.Runtime
{
    internal sealed class EntityControllerTests : Injectable
    {
        [Inject] private IEntityController _entityController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Assert.That(Application.isPlaying, Is.True, "[EntityControllerTests::OneTimeSetUp] " +
                                                        "These tests must be run in play mode!");
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            while (StartUpController.IsInited is false)
            {
                yield return null;
            }

            Debug.Log("[EntityControllerTests::OneTimeSetUp] Scene loaded, RuntimeInitialize methods executed");

            yield return ClearAllEntities();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return ClearAllEntities();
        }

        private IEnumerator ClearAllEntities()
        {
            if (AppConfig.Instance.CleanUpTests is false)
                yield break;

            var userEntries
                = _entityController.FindEntries<UserEntity, UserEntryContent>(_ => true);

            if (userEntries != null)
            {
                foreach (var entry in userEntries)
                {
                    var deleteTask = _entityController.DeleteEntryAsync<UserEntity, UserEntryContent>(entry.Id);
                    yield return deleteTask.WaitForTask();
                }
            }

            var categoryEntries = _entityController
                .FindEntries<UserCategoryEntity, UserCategoryEntryContent>(_ => true);

            if (userEntries != null)
            {
                foreach (var entry in categoryEntries)
                {
                    var deleteTask = _entityController
                        .DeleteEntryAsync<UserCategoryEntity, UserCategoryEntryContent>(entry.Id);
                    yield return deleteTask.WaitForTask();
                }
            }

            var wordEntries = _entityController
                .FindEntries<WordEntity, WordEntryContent>(_ => true);

            if (wordEntries != null)
            {
                foreach (var entry in wordEntries)
                {
                    var deleteTask = _entityController.DeleteEntryAsync<WordEntity, WordEntryContent>(entry.Id);
                    yield return deleteTask.WaitForTask();
                }
            }

            var progressEntries = _entityController
                .FindEntries<ProgressEntity, ProgressEntryContent>(_ => true);

            if (progressEntries != null)
            {
                foreach (var entry in progressEntries)
                {
                    var deleteTask = _entityController
                        .DeleteEntryAsync<ProgressEntity, ProgressEntryContent>(entry.Id);
                    yield return deleteTask.WaitForTask();
                }
            }
        }

        public sealed class TestParams<TEntity, TContent>
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            public Type EntityType { get; set; }
            public TContent Content { get; set; }
        }

        #region Write Arrange

        private static IEnumerable<TestCaseData> ValidEntityTestCases()
        {
            yield return new TestCaseData(
                new TestParams<UserEntity, UserEntryContent>
                {
                    EntityType = typeof(UserEntity),
                    Content = new UserEntryContent
                    {
                        UserName = "TestUser",
                        Email = "test@example.com",
                        Password = "pwdpwdp"
                    }
                }
            ).Returns(null).SetName("UserEntity_Create");

            yield return new TestCaseData(
                new TestParams<UserCategoryEntity, UserCategoryEntryContent>
                {
                    EntityType = typeof(UserCategoryEntity),
                    Content = new UserCategoryEntryContent
                    {
                        Title = "Test Category",
                        Description = "Test Description"
                    }
                }
            ).Returns(null).SetName("CategoryEntity_Create");

            yield return new TestCaseData(
                new TestParams<WordEntity, WordEntryContent>
                {
                    EntityType = typeof(WordEntity),
                    Content = new WordEntryContent
                    {
                        CategoryId = "testCategoryId",
                        NativeWord = "Hello",
                        LearningWord = "Hola",
                        Transcription = "həˈləʊ"
                    }
                }
            ).Returns(null).SetName("WordEntity_Create");

            yield return new TestCaseData(
                new TestParams<ProgressEntity, ProgressEntryContent>
                {
                    EntityType = typeof(ProgressEntity),
                    Content = new ProgressEntryContent
                    {
                        WordId = "test-id",
                        RepetitionStage = 1,
                        TotalReviews = 0
                    }
                }
            ).Returns(null).SetName("ProgressEntity_Create");
        }

        private static IEnumerable<TestCaseData> WrongEntityTestCases()
        {
            yield return new TestCaseData(
                    new TestParams<UserEntity, UserEntryContent>
                    {
                        EntityType = typeof(ProgressEntity),
                        Content = new UserEntryContent
                        {
                            UserName = "",
                            Password = " "
                        }
                    }
                ).Returns(null)
                .SetName("UserEntity_Create")
                .SetProperty("ExpectedLog",
                    "[EntityBase::CreateEntryAsync] Validation failed: " +
                    "UserName: Length must be at least 1, " +
                    "UserName: Value cannot be empty or whitespace, " +
                    "Password: Length must be at least 6, " +
                    "Password: Value cannot be empty or whitespace, " +
                    "Email: Expected string but got null, " +
                    "Email: Value cannot be null");
            yield return new TestCaseData(
                    new TestParams<UserCategoryEntity, UserCategoryEntryContent>
                    {
                        EntityType = typeof(UserCategoryEntity),
                        Content = new UserCategoryEntryContent
                        {
                            Description = "Test Description"
                        }
                    }
                ).Returns(null)
                .SetName("CategoryEntity_Create")
                .SetProperty("ExpectedLog",
                    "[EntityBase::CreateEntryAsync] Validation failed: " +
                    "Title: Expected string but got null, " +
                    "Title: Value cannot be null");

            yield return new TestCaseData(
                    new TestParams<WordEntity, WordEntryContent>
                    {
                        EntityType = typeof(WordEntity),
                        Content = new WordEntryContent
                        {
                            NativeWord = "",
                            LearningWord = " ",
                            Transcription = "həˈləʊ"
                        }
                    }).Returns(null)
                .SetName("WordEntity_Create")
                .SetProperty("ExpectedLog",
                    "[EntityBase::CreateEntryAsync] Validation failed: " +
                    "CategoryId: Value cannot be null, " +
                    "NativeWord: Length must be at least 1, " +
                    "NativeWord: Value cannot be empty or whitespace, " +
                    "LearningWord: Value cannot be empty or whitespace");

            yield return new TestCaseData(
                    new TestParams<ProgressEntity, ProgressEntryContent>
                    {
                        EntityType = typeof(ProgressEntity),
                        Content = new ProgressEntryContent
                        {
                            WordId = null,
                            RepetitionStage = 0,
                            TotalReviews = 0
                        }
                    }
                ).Returns(null)
                .SetName("ProgressEntity_Create")
                .SetProperty("ExpectedLog",
                    "[EntityBase::CreateEntryAsync] Validation failed: " +
                    "WordId: Expected string but got null, " +
                    "WordId: Value cannot be null, " +
                    "RepetitionStage: Value must be at least 1, " +
                    "RepetitionStage: Value cannot be zero");
        }

        #endregion

        #region Write Tests

        [UnityTest]
        [TestCaseSource(nameof(ValidEntityTestCases))]
        public IEnumerator CreateEntity_WithValidData_ShouldSucceed<TEntity, TContent>
            (TestParams<TEntity, TContent> testParams)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            // Act
            var createTask = _entityController.CreateEntryAsync<TEntity, TContent>(testParams.Content);
            yield return createTask.WaitForTask();

            var result = createTask.Result;

            // Assert
            Assert.That(result.EntryData.Content, Is.EqualTo(testParams.Content), result.ErrorMessage);
            Assert.That(result.IsSuccess, Is.True, result.ErrorMessage);
            Assert.That(result.Exception, Is.Null, result.ErrorMessage);
        }

        [UnityTest]
        [TestCaseSource(nameof(WrongEntityTestCases))]
        public IEnumerator CreateEntity_WithWrongData_ShouldFail<TEntity, TContent>
            (TestParams<TEntity, TContent> testParams)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (TestContext.CurrentContext.Test.Properties.Get("ExpectedLog") is string expectedLog)
                LogAssert.Expect(LogType.Error, expectedLog);

            // Act
            var createTask = _entityController.CreateEntryAsync<TEntity, TContent>(testParams.Content);
            yield return createTask.WaitForTask();

            var result = createTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.False, result.ErrorMessage);
            Assert.That(result.EntryData, Is.Null, result.ErrorMessage);
        }

        #endregion

        #region Read Tests

        [UnityTest]
        public IEnumerator ReadEntity_AfterCreation_ShouldReturnData()
        {
            // Arrange
            var categoryData = new UserCategoryEntryContent
            {
                Title = "Test Category",
                Description = "Test Description",
            };

            var createTask =
                _entityController.CreateEntryAsync<UserCategoryEntity, UserCategoryEntryContent>(categoryData);
            yield return createTask.WaitForTask();

            // Act
            var readTask = _entityController
                .ReadEntryAsync<UserCategoryEntity, UserCategoryEntryContent>(createTask.Result.EntryData.Id);
            yield return readTask.WaitForTask();

            var result = readTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.EntryData, Is.Not.Null);
            Assert.That(result.EntryData.Content.Title, Is.EqualTo("Test Category"));
        }

        #endregion

        #region Update Tests

        [UnityTest]
        public IEnumerator UpdateEntity_WithValidData_ShouldSucceed()
        {
            // Arrange
            var wordData = new WordEntryContent
            {
                CategoryId = "testCategoryId",
                NativeWord = "Hello",
                LearningWord = "Bonjour",
                Transcription = "bɔ̃ʒuʁ"
            };

            var createTask = _entityController.CreateEntryAsync<WordEntity, WordEntryContent>(wordData);
            yield return createTask.WaitForTask();

            wordData.Transcription = "Updated Transcription";

            // Act
            var updateTask = _entityController.UpdateEntryAsync<WordEntity, WordEntryContent>
                (createTask.Result.EntryData, wordData);
            yield return updateTask.WaitForTask();

            var result = updateTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.EntryData, Is.Not.Null);
            Assert.That(result.EntryData.Content.Transcription, Is.EqualTo("Updated Transcription"));
        }

        #endregion UpdateTests

        #region Delete Tests

        [UnityTest]
        public IEnumerator DeleteEntity_ExistingEntity_ShouldSucceed()
        {
            // Arrange
            var progressData = new ProgressEntryContent
            {
                WordId = "test-word-id",
                RepetitionStage = 1,
                TotalReviews = 5
            };

            var createTask = _entityController.CreateEntryAsync<ProgressEntity, ProgressEntryContent>(progressData);
            yield return createTask.WaitForTask();

            var entityId = createTask.Result.EntryData.Id;

            // Act
            var deleteTask = _entityController.DeleteEntryAsync<ProgressEntity, ProgressEntryContent>(entityId);
            yield return deleteTask.WaitForTask();

            var result = deleteTask.Result;

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.EntryData, Is.Not.Null);
        }

        #endregion

        #region Controller helpers Tests

        [UnityTest]
        public IEnumerator ExistsAsync_WithExistingEntity_ShouldReturnTrue()
        {
            // Arrange
            var userData = new UserEntryContent
            {
                UserName = "ExistingUser",
                Email = "existing@example.com",
                Password = "1234567"
            };

            var createTask = _entityController.CreateEntryAsync<UserEntity, UserEntryContent>(userData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            // Act
            var existsTask =
                _entityController.ExistsAsync<UserEntity, UserEntryContent>(createTask.Result.EntryData.Id);
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
            var wordEntry1 = new WordEntryContent
            {
                CategoryId = "TestId",
                NativeWord = "Hello",
                LearningWord = "Bonjour"
            };
            var wordEntry2 = new WordEntryContent
            {
                CategoryId = "TestId",
                NativeWord = "Goodbye",
                LearningWord = "Au revoir"
            };

            var result1
                = _entityController.CreateEntryAsync<WordEntity, WordEntryContent>(wordEntry1);
            var result2
                = _entityController.CreateEntryAsync<WordEntity, WordEntryContent>(wordEntry2);
            while (result1.IsCompleted is false || result2.IsCompleted is false)
            {
                yield return null;
            }

            // Act
            var results = _entityController
                .FindEntries<WordEntity, WordEntryContent>(
                    w => w.Content.LearningWord.Contains("Bonjour")
                );

            // Assert with detailed logging
            Assert.That(results, Is.Not.Empty);
            Assert.That(results.Length, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator EntityEvents_ShouldTriggerAppropriately()
        {
            // Arrange
            var eventTriggered = false;
            _entityController.OnEntryCreated += (_, _) => eventTriggered = true;

            var userData = new UserEntryContent
            {
                UserName = "EventTestUser",
                Email = "event@example.com",
                Password = "123456"
            };

            // Act
            var createTask = _entityController.CreateEntryAsync<UserEntity, UserEntryContent>(userData);
            while (!createTask.IsCompleted)
            {
                yield return null;
            }

            // Assert
            Assert.That(eventTriggered, Is.True, "Entry created event should have been triggered");
        }

        #endregion
    }
}