using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Core;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.CategoryEntity;
using Client.Scripts.DB.Entities.ProgressEntity;
using Client.Scripts.DB.Entities.UserEntity;
using Client.Scripts.DB.Entities.WordEntity;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Client.Tests.Runtime
{
    internal class EntityBaseTests : Injectable
    {
        private readonly List<EntryData<UserEntryContent>> _createdUserEntities = new();
        private readonly List<EntryData<UserCategoryEntryContent>> _createdUserCategoryEntities = new();
        private readonly List<EntryData<GlobalCategoryEntryContent>> _createdGlobalCategoryEntities = new();
        private readonly List<EntryData<WordEntryContent>> _createdWordEntities = new();
        private readonly List<EntryData<ProgressEntryContent>> _createdProgressEntities = new();
        
        private UserEntity _userEntity;
        private UserCategoryEntity _userCategoryEntity;
        private GlobalCategoryEntity _globalCategoryEntity;
        private WordEntity _wordEntity;
        private ProgressEntity _progressEntity;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userEntity = new UserEntity();
            _userCategoryEntity = new UserCategoryEntity();
            _globalCategoryEntity = new GlobalCategoryEntity();
            _wordEntity = new WordEntity();
            _progressEntity = new ProgressEntity();

            Assert.That(Application.isPlaying, Is.True, "Tests must run in play mode!");
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return _userEntity.InitAsync().WaitForTask();
            yield return _userCategoryEntity.InitAsync().WaitForTask();
            yield return _globalCategoryEntity.InitAsync().WaitForTask();
            yield return _wordEntity.InitAsync().WaitForTask();
            yield return _progressEntity.InitAsync().WaitForTask();

            Debug.Log("[EntityControllerTests::OneTimeSetUp] Scene loaded, RuntimeInitialize methods executed");
        }

        [UnityTearDown]
        public IEnumerator Cleanup()
        {
            if (AppConfig.Instance.CleanUpTests is false)
                yield break;

            foreach (var entity in _createdUserEntities)
                yield return _userEntity.DeleteEntryAsync(entity).WaitForTask();

            foreach (var entity in _createdUserCategoryEntities)
                yield return _userCategoryEntity.DeleteEntryAsync(entity).WaitForTask();

            foreach (var entity in _createdGlobalCategoryEntities)
                yield return _globalCategoryEntity.DeleteEntryAsync(entity).WaitForTask();
            
            foreach (var entity in _createdWordEntities)
                yield return _wordEntity.DeleteEntryAsync(entity).WaitForTask();

            foreach (var entity in _createdProgressEntities)
                yield return _progressEntity.DeleteEntryAsync(entity).WaitForTask();

            _createdUserEntities.Clear();
            _createdUserCategoryEntities.Clear();
            _createdGlobalCategoryEntities.Clear();
            _createdWordEntities.Clear();
            _createdProgressEntities.Clear();
        }

        #region Loading Tests

        [UnityTest]
        public IEnumerator LoadAllEntries_ShouldSucceed()
        {
            // Arrange
            var desiredWordData = new WordEntryContent
            {
                CategoryId = "TestCategory",
                NativeWord = "FirstWord",
                LearningWord = "LearningWord",
                Transcription = "Transcription",
                Examples = new List<WordEntryContent.Example>
                {
                    new WordEntryContent.Example
                    {
                        NativeSentence = "NativeSentence",
                        LearningSentence = "LearningSentence"
                    }
                },
                IsDefault = false
            };

            var wrongWordData = new WordEntryContent
            {
                CategoryId = "TestCategory",
                NativeWord = "SecondWord",
                LearningWord = "LearningWord",
                Transcription = "Transcription",
                Examples = new List<WordEntryContent.Example>
                {
                    new WordEntryContent.Example
                    {
                        NativeSentence = "NativeSentence",
                        LearningSentence = "LearningSentence"
                    }
                },
                IsDefault = false
            };

            // Act
            var createTask1 = _wordEntity.CreateEntryAsync(desiredWordData);
            yield return createTask1.WaitForTask();

            var createTask2 = _wordEntity.CreateEntryAsync(wrongWordData);
            yield return createTask2.WaitForTask();

            var createResult1 = createTask1.Result;
            var createResult2 = createTask2.Result;
            if (createResult1 != null || createResult2 != null)
            {
                _createdWordEntities.Add(createResult1);
                _createdWordEntities.Add(createResult2);
            }

            var loadedTask = _wordEntity.LoadEntryAsync();
            yield return loadedTask.WaitForTask();

            // Assert
            Assert.That(_wordEntity.Entries, Is.Not.Null);
            Assert.That(_wordEntity.Entries.Count, Is.EqualTo(2));
            Assert.That(_wordEntity.Entries.Values.Count(entry => entry.Content.NativeWord == "FirstWord"),
                Is.EqualTo(1));
            Assert.That(_wordEntity.Entries.Values.Count(entry => entry.Content.NativeWord == "SecondWord"),
                Is.EqualTo(1));
        }

        #endregion

        #region Create Tests

        [UnityTest]
        public IEnumerator CreateEntity_WithValidData_ShouldSucceed()
        {
            // Arrange
            var userData = new UserEntryContent
            {
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var createTask = _userEntity.CreateEntryAsync(userData);
            yield return createTask.WaitForTask();

            var result = createTask.Result;
            if (result != null)
                _createdUserEntities.Add(result);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.UserName, Is.EqualTo("TestUser"));
            Assert.That(_userEntity.Entries.Count, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator CreateEntity_WithInvalidData_ShouldFail()
        {
            // Arrange
            var userData = new UserEntryContent
            {
                UserName = "", // Invalid: empty username
                Email = "test@example.com",
                Password = "pwd" // Invalid: too short
            };

            LogAssert.Expect(LogType.Error,
                "[EntityBase::CreateEntryAsync] Validation failed: " +
                "UserName: Length must be at least 1, " +
                "UserName: Value cannot be empty or whitespace, " +
                "Password: Length must be at least 6");

            // Act
            var createTask = _userEntity.CreateEntryAsync(userData);
            yield return createTask.WaitForTask();


            // Assert
            Assert.That(createTask.Result, Is.Null);
            Assert.That(_userEntity.Entries.Count, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator CreateSingleInstance_MultipleTimes_ShouldFail()
        {
            // Arrange
            var progress1 = new ProgressEntryContent
            {
                WordId = "word-1",
                RepetitionStage = 1,
                TotalReviews = 0
            };

            var progress2 = new ProgressEntryContent
            {
                WordId = "word-2",
                RepetitionStage = 1,
                TotalReviews = 0
            };

            LogAssert.Expect(LogType.Error, "[EntityBase::CreateEntryAsync] " +
                                            "Cannot create multiple instances of ProgressEntity");

            // Act
            var createTask1 = _progressEntity.CreateEntryAsync(progress1);
            yield return createTask1.WaitForTask();

            var result1 = createTask1.Result;
            if (result1 != null)
                _createdProgressEntities.Add(result1);

            var createTask2 = _progressEntity.CreateEntryAsync(progress2);
            yield return createTask2.WaitForTask();


            // Assert
            Assert.That(result1, Is.Not.Null);
            Assert.That(createTask2.Result, Is.Null);
            Assert.That(_progressEntity.Entries.Count, Is.EqualTo(1));
        }

        #endregion

        #region Update Tests

        [UnityTest]
        public IEnumerator UpdateEntity_WithValidData_ShouldSucceed()
        {
            // Arrange
            var categoryData = new UserCategoryEntryContent
            {
                Title = "Original",
                Description = "Test category",
            };

            var createTask = _userCategoryEntity.CreateEntryAsync(categoryData);
            yield return createTask.WaitForTask();

            var entity = createTask.Result;
            Assert.That(entity, Is.Not.Null);

            _createdUserCategoryEntities.Add(entity);
            entity.Content.Title = "Updated";

            // Act
            var updateTask = _userCategoryEntity.UpdateEntryAsync(entity);
            yield return updateTask.WaitForTask();

            // Assert
            Assert.That(updateTask.Result, Is.Not.Null);
            Assert.That(updateTask.Result.Content.Title, Is.EqualTo("Updated"));
        }

        [UnityTest]
        public IEnumerator UpdateEntity_WithInvalidData_ShouldFail()
        {
            // Arrange
            var wordData = new WordEntryContent
            {
                CategoryId = "category-1",
                NativeWord = "Original",
                LearningWord = "Translation",
                Transcription = "test"
            };

            var createTask = _wordEntity.CreateEntryAsync(wordData);
            yield return createTask.WaitForTask();

            var entity = createTask.Result;

            _createdWordEntities.Add(entity);

            entity.Content.NativeWord = ""; // Invalid: empty word

            LogAssert.Expect(LogType.Error,
                "[EntityBase::UpdateEntryAsync] Validation failed: " +
                "NativeWord: Length must be at least 1, " +
                "NativeWord: Value cannot be empty or whitespace");

            // Assert
            Assert.That(entity, Is.Not.Null);

            // Act
            var updateTask = _wordEntity.UpdateEntryAsync(entity);
            yield return updateTask.WaitForTask();

            var readTask = _wordEntity.ReadEntryAsync(entity.Id);
            yield return readTask.WaitForTask();

            // Assert
            Assert.That(updateTask.Result, Is.Null);
            Assert.That(readTask.Result.Content.NativeWord, Is.EqualTo("Original"));
        }

        #endregion

        #region Delete Tests

        [UnityTest]
        public IEnumerator DeleteEntity_ExistingEntity_ShouldSucceed()
        {
            // Arrange
            var categoryData = new UserCategoryEntryContent
            {
                Title = "ToDelete",
                Description = "Test category",
            };

            var createTask = _userCategoryEntity.CreateEntryAsync(categoryData);
            yield return createTask.WaitForTask();

            var entity = createTask.Result;

            // Assert
            Assert.That(entity, Is.Not.Null);

            // Act
            var deleteTask = _userCategoryEntity.DeleteEntryAsync(entity);
            yield return deleteTask.WaitForTask();

            // Assert
            Assert.That(_userCategoryEntity.Entries.Count, Is.EqualTo(0));
        }

        #endregion

        #region Read Tests

        [UnityTest]
        public IEnumerator ReadEntity_AfterCreation_ShouldReturnData()
        {
            // Arrange
            var wordData = new WordEntryContent
            {
                CategoryId = "category-1",
                NativeWord = "Test",
                LearningWord = "Translation",
                Transcription = "test"
            };

            var createTask = _wordEntity.CreateEntryAsync(wordData);
            yield return createTask.WaitForTask();

            var entity = createTask.Result;
            if (entity != null)
                _createdWordEntities.Add(entity);

            // Act
            var readTask = _wordEntity.ReadEntryAsync(entity?.Id);
            yield return readTask.WaitForTask();

            // Assert
            Assert.That(readTask.Result, Is.Not.Null);
            Assert.That(readTask.Result.Content.NativeWord, Is.EqualTo("Test"));
        }

        #endregion

        #region Entries Tests

        [UnityTest]
        public IEnumerator EntriesProperty_AfterCreation_ShouldReturnSomeData()
        {
            // Arrange
            var wordData = new WordEntryContent
            {
                CategoryId = "category-1",
                NativeWord = "Test",
                LearningWord = "Translation",
                Transcription = "test"
            };

            var createTask = _wordEntity.CreateEntryAsync(wordData);
            yield return createTask.WaitForTask();

            var entity = createTask.Result;
            if (entity != null)
                _createdWordEntities.Add(entity);

            // Act
            var entries = _wordEntity.Entries;
            var entryData = entries.Values.First();

            // Assert
            Assert.That(entries, Is.Not.Null);
            Assert.That(entries, Is.Not.Empty);
            Assert.That(entryData.Content.NativeWord, Is.EqualTo("Test"));
        }

        #endregion
    }
}