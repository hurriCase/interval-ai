using System.Threading.Tasks;
using Client.Scripts.DB.DBControllers;
using Client.Scripts.DB.Entities.UserEntity;
using Client.Scripts.Patterns.DI.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Client.Tests.Editor
{
    internal sealed class FireBaseRepositoryTests
    {
        private FireBaseRepository _controller;
        private string TestPath => DBConfig.Instance.TestsPath;

        [SetUp]
        public void Setup()
        {
            _controller = new FireBaseRepository();
        }

        #region Write Tests

        [Test]
        public async Task WriteData_WhenNotInitialized_ShouldFail()
        {
            // Arrange
            var testData = "test";

            LogAssert.Expect(LogType.Error,
                "[FireBaseDB::CheckDBInit] FireBaseDB not initialized but you're trying to access");

            // Act
            var result = await _controller.WriteDataAsync(DataType.User, TestPath, testData);


            // Assert
            Assert.That(result, Is.EqualTo(default(string)));
        }

        [Test]
        public async Task WriteString_WhenInitializedAndValid_ShouldSucceed()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();

            // Act
            var result = await _controller.WriteDataAsync(DataType.User, TestPath, testData);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task WriteJsonData_WhenInitializedAndValid_ShouldSucceed()
        {
            // Arrange
            var testData = new UserEntryContent
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUserJson";
            var json = JsonConvert.SerializeObject(testData);

            // Act
            var result = await _controller.WriteDataAsync(DataType.User, TestPath, testData);

            // Assert
            Assert.That(result, Is.EqualTo(json));
        }

        #endregion

        #region Read Tests

        [Test]
        public async Task ReadString_AfterWrite_ShouldReturnSameData()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();
            _controller.UserID = "testUser";

            // Act
            await _controller.WriteDataAsync(DataType.User, TestPath, testData);
            var result = await _controller.ReadDataAsync<string>(DataType.User, TestPath);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task ReadJsonData_AfterWrite_ShouldReturnSameJsonData()
        {
            // Arrange
            var testData = new UserEntryContent
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUserJson";

            // Act
            await _controller.WriteDataAsync(DataType.User, TestPath, testData);
            var result = await _controller.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(result.UserName, Is.EqualTo(testData.UserName));
            Assert.That(result.Password, Is.EqualTo(testData.Password));
            Assert.That(result.Email, Is.EqualTo(testData.Email));
        }

        #endregion

        #region Update Tests

        [Test]
        public async Task UpdateString_AfterWrite_ShouldUpdateAndReturnSameData()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();
            _controller.UserID = "testUser";

            // Act
            await _controller.WriteDataAsync(DataType.User, TestPath, testData);
            var readDataBeforeUpdate = await _controller.ReadDataAsync<string>(DataType.User, TestPath);

            // Assert
            Assert.That(readDataBeforeUpdate, Is.EqualTo(testData));

            // Act
            var newData = "newData";
            await _controller.UpdateDataAsync(DataType.User, TestPath, newData);
            var readDataAfterUpdate = await _controller.ReadDataAsync<string>(DataType.User, TestPath);

            // Assert
            Assert.That(readDataAfterUpdate, Is.EqualTo(newData));
        }

        [Test]
        public async Task UpdateJsonData_AfterWrite_ShouldUpdateAndReturnSameData()
        {
            // Arrange
            var testData = new UserEntryContent
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUser";

            // Act
            await _controller.WriteDataAsync(DataType.User, TestPath, testData);
            var readDataBeforeUpdate = await _controller.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(readDataBeforeUpdate.UserName, Is.EqualTo("testUser"));
            Assert.That(readDataBeforeUpdate.Password, Is.EqualTo("testPassword"));
            Assert.That(readDataBeforeUpdate.Email, Is.EqualTo("testEmail"));

            // Act
            readDataBeforeUpdate.UserName = "newData";
            await _controller.UpdateDataAsync(DataType.User, TestPath, readDataBeforeUpdate);
            var readDataAfterUpdate = await _controller.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(readDataAfterUpdate.UserName, Is.EqualTo("newData"));
            Assert.That(readDataAfterUpdate.Password, Is.EqualTo("testPassword"));
            Assert.That(readDataAfterUpdate.Email, Is.EqualTo("testEmail"));
        }

        #endregion

        #region Delete Tests

        [Test]
        public async Task DeleteString_ShouldRemoveData()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();
            _controller.UserID = "testUser";
            await _controller.WriteDataAsync(DataType.User, TestPath, testData);
            var beforeDelete = await _controller.ReadDataAsync<string>(DataType.User, TestPath);

            // Assert
            Assert.That(beforeDelete, Is.EqualTo(testData));

            // Act
            await _controller.DeleteDataAsync(DataType.User, TestPath);
            var afterDelete = await _controller.ReadDataAsync<string>(DataType.User, TestPath);

            // Assert
            Assert.That(afterDelete, Is.EqualTo(default(string)));
        }

        [Test]
        public async Task DeleteJsonData_ShouldRemoveData()
        {
            // Arrange
            var testData = new UserEntryContent
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUserJson";

            // Act
            await _controller.WriteDataAsync(DataType.User, TestPath, testData);
            var beforeDelete = await _controller.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(beforeDelete.UserName, Is.EqualTo(testData.UserName));
            Assert.That(beforeDelete.Password, Is.EqualTo(testData.Password));
            Assert.That(beforeDelete.Email, Is.EqualTo(testData.Email));

            // Act
            await _controller.DeleteDataAsync(DataType.User, TestPath);
            var afterDelete = await _controller.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(afterDelete, Is.EqualTo(null));
        }

        #endregion

        [TearDown]
        public async Task Cleanup()
        {
            if (_controller.UserID != null)
                await _controller.DeleteDataAsync(DataType.User, TestPath);
        }
    }
}