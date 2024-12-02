using System.Threading.Tasks;
using Client.Scripts.DB.Base;
using Client.Scripts.DB.Entities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Client.Tests.Editor.DBTests
{
    internal class DBControllerTests
    {
        private DBController _controller;
        private const string TestPath = "test/path";

        [SetUp]
        public void Setup()
        {
            _controller = new DBController();
        }

        [Test]
        public async Task WriteData_WhenNotInitialized_ShouldFail()
        {
            // Arrange
            LogAssert.Expect(LogType.Error,
                "[DBController::ValidateDB] DBController not initialized but you're trying to access");
            var testData = "test";

            // Act
            var result = await _controller.WriteDataAsync(TestPath, testData);

            // Assert
            Assert.That(result, Is.EqualTo(default(string)));
        }

        [Test]
        public async Task WriteString_WhenInitializedAndValid_ShouldSucceed()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();
            _controller.UserID = "testUser";

            // Act
            var result = await _controller.WriteDataAsync(TestPath, testData);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task WriteJsonData_WhenInitializedAndValid_ShouldSucceed()
        {
            // Arrange
            var testData = new UserEntityData
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUserJson";

            // Act
            var result = await _controller.WriteDataAsync(TestPath, testData);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task ReadString_AfterWrite_ShouldReturnSameData()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();
            _controller.UserID = "testUser";

            // Act
            await _controller.WriteDataAsync(TestPath, testData);
            var result = await _controller.ReadDataAsync<string>(TestPath);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task ReadJsonData_AfterWrite_ShouldReturnSameJsonData()
        {
            // Arrange
            var testData = new UserEntityData
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUserJson";

            // Act
            await _controller.WriteDataAsync(TestPath, testData);
            var result = await _controller.ReadDataAsync<UserEntityData>(TestPath);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task DeleteString_ShouldRemoveData()
        {
            // Arrange
            var testData = "test";
            await _controller.InitAsync();
            _controller.UserID = "testUser";
            await _controller.WriteDataAsync(TestPath, testData);
            var beforeDelete = await _controller.ReadDataAsync<string>(TestPath);
            Assert.That(beforeDelete, Is.EqualTo(testData));

            // Act
            await _controller.DeleteDataAsync(TestPath);
            var afterDelete = await _controller.ReadDataAsync<string>(TestPath);

            // Assert
            Assert.That(afterDelete, Is.EqualTo(default(string)));
        }

        [Test]
        public async Task DeleteJsonData_ShouldRemoveData()
        {
            // Arrange
            var testData = new UserEntityData
            {
                UserName = "testUser",
                Password = "testPassword",
                Email = "testEmail"
            };
            await _controller.InitAsync();
            _controller.UserID = "testUserJson";
            await _controller.WriteDataAsync(TestPath, testData);
            var beforeDelete = await _controller.ReadDataAsync<UserEntityData>(TestPath);
            Assert.That(beforeDelete, Is.EqualTo(testData));

            // Act
            await _controller.DeleteDataAsync(TestPath);
            var afterDelete = await _controller.ReadDataAsync<UserEntityData>(TestPath);

            // Assert
            Assert.That(afterDelete, Is.EqualTo(default(UserEntityData)));
        }

        [TearDown]
        public async Task Cleanup()
        {
            if (_controller.UserID != null)
            {
                await _controller.DeleteDataAsync(TestPath);
            }
        }
    }
}