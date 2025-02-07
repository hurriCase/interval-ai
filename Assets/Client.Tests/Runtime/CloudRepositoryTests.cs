using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Core;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.Entities.User;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using NUnit.Framework;

namespace Client.Tests.Runtime
{
    internal sealed class CloudRepositoryTests : Injectable
    {
        [Inject] private ICloudRepository _cloudRepository;
        private string TestPath => DBConfig.Instance.TestsPath;

        #region Write Tests

        [Test]
        public async Task WriteString_WhenInitializedAndValid_ShouldSucceed()
        {
            // Arrange
            var testData = new Dictionary<string, string>
            {
                { "testKey", "testData" }
            };
            await _cloudRepository.InitAsync();

            // Act
            var result = await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);

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
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUserJson";

            // Act
            var result = await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(testData));
        }

        #endregion

        #region Read Tests

        [Test]
        public async Task ReadString_AfterWrite_ShouldReturnSameData()
        {
            // Arrange
            var testData = new Dictionary<string, string>
            {
                { "testKey", "testData" }
            };
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUser";

            // Act
            await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);
            var result = await _cloudRepository.ReadDataAsync<Dictionary<string, string>>(DataType.User, TestPath);

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
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUserJson";

            // Act
            await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);
            var result = await _cloudRepository.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

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
            var testData = new Dictionary<string, string>
            {
                { "testKey", "testData" }
            };
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUser";

            // Act
            await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);
            var readDataBeforeUpdate =
                await _cloudRepository.ReadDataAsync<Dictionary<string, string>>(DataType.User, TestPath);

            // Assert
            Assert.That(readDataBeforeUpdate, Is.EqualTo(testData));

            // Act
            var newData = new Dictionary<string, string>
            {
                { "testKey", "newTestData" }
            };
            await _cloudRepository.UpdateDataAsync(DataType.User, TestPath, newData);
            var readDataAfterUpdate =
                await _cloudRepository.ReadDataAsync<Dictionary<string, string>>(DataType.User, TestPath);

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
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUser";

            // Act
            await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);
            var readDataBeforeUpdate = await _cloudRepository.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(readDataBeforeUpdate.UserName, Is.EqualTo("testUser"));
            Assert.That(readDataBeforeUpdate.Password, Is.EqualTo("testPassword"));
            Assert.That(readDataBeforeUpdate.Email, Is.EqualTo("testEmail"));

            // Act
            readDataBeforeUpdate.UserName = "newData";
            await _cloudRepository.UpdateDataAsync(DataType.User, TestPath, readDataBeforeUpdate);
            var readDataAfterUpdate = await _cloudRepository.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

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
            var testData = new Dictionary<string, string>
            {
                { "testKey", "testData" }
            };
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUser";
            await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);
            var beforeDelete =
                await _cloudRepository.ReadDataAsync<Dictionary<string, string>>(DataType.User, TestPath);

            // Assert
            Assert.That(beforeDelete, Is.EqualTo(testData));

            // Act
            await _cloudRepository.DeleteDataAsync(DataType.User, TestPath);
            var afterDelete = await _cloudRepository.ReadDataAsync<Dictionary<string, string>>(DataType.User, TestPath);

            // Assert
            Assert.That(afterDelete, Is.EqualTo(null));
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
            await _cloudRepository.InitAsync();
            UserData.Instance.UserID = "testUserJson";

            // Act
            await _cloudRepository.WriteDataAsync(DataType.User, TestPath, testData);
            var beforeDelete = await _cloudRepository.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(beforeDelete.UserName, Is.EqualTo(testData.UserName));
            Assert.That(beforeDelete.Password, Is.EqualTo(testData.Password));
            Assert.That(beforeDelete.Email, Is.EqualTo(testData.Email));

            // Act
            await _cloudRepository.DeleteDataAsync(DataType.User, TestPath);
            var afterDelete = await _cloudRepository.ReadDataAsync<UserEntryContent>(DataType.User, TestPath);

            // Assert
            Assert.That(afterDelete, Is.EqualTo(null));
        }

        #endregion

        [TearDown]
        public async Task Cleanup()
        {
            if (AppConfig.Instance.CleanUpTests)
                await _cloudRepository.DeleteDataAsync(DataType.User, TestPath);
        }
    }
}