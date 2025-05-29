using System;
using System.Threading.Tasks;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.DataRepositories.Offline;
using CustomUtils.Runtime.CustomTypes.Singletons;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using UnityEngine;

namespace Client.Scripts.DB.Data
{
    internal sealed class UserDataController : Singleton<UserDataController>, IInjectable, IUserDataController
    {
        [Inject] private IOfflineRepository _offlineRepository;
        [Inject] private ICloudRepository _cloudRepository;
        private UserData UserData => UserData.Instance;

        private bool _isInited;

        public async void Init()
        {
            try
            {
                if (_isInited) return;

                InjectDependencies();

                UserData.Instance.OnValueChanged += SaveUserData;

                var offlineData = await _offlineRepository.ReadDataAsync<UserDataTransferObject>(
                    DataType.User,
                    DBConfig.Instance.UserDataPath
                );

                if (offlineData != null)
                    UserData.LoadFromDataTransferObject(offlineData);

                if (offlineData is null || UserData.LogInType == LogInType.Guest)
                    InitAsGuest();

                _isInited = true;

                Debug.Log($"[UserData::Init] Initialized user data. Mode: {UserData.LogInType}, ID: {UserData.UserID}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UserData::Init] Failed to initialize user data, with error: {e.Message}");
                InitAsGuest();
            }
        }

        public void InitAsGuest()
        {
            try
            {
                if (string.IsNullOrEmpty(UserData.TemporaryGuestId))
                    UserData.TemporaryGuestId = Guid.NewGuid().ToString();

                UserData.UserID = UserData.TemporaryGuestId;
                UserData.LogInType = LogInType.Guest;
            }
            catch (Exception e)
            {
                Debug.LogError($"[UserDataController::InitAsGuest] Failed to init guest, with error: {e.Message}");
            }
        }

        public async Task TransitionFromGuestToAuthenticated(string authenticatedUserId)
        {
            if (UserData.LogInType != LogInType.Guest || string.IsNullOrEmpty(UserData.TemporaryGuestId))
            {
                Debug.LogWarning("Attempting to transition non-guest user or missing temporary ID");
                return;
            }

            try
            {
                var migrationResult = await MigrateUserData(UserData.TemporaryGuestId, authenticatedUserId);
                if (migrationResult.Success == false)
                    throw new Exception(migrationResult.Error);

                UserData.UserID = authenticatedUserId;
                UserData.LogInType = LogInType.GoogleSignIn;
                UserData.TemporaryGuestId = null;

                Debug.Log("[UserDataController::TransitionFromGuestToAuthenticated] " +
                          $"Successfully transitioned guest user to authenticated user, with error: {authenticatedUserId}");
            }
            catch (Exception e)
            {
                Debug.LogError("[UserDataController::TransitionFromGuestToAuthenticated] " +
                               $"Failed to transition from guest to authenticated user, with error: {e.Message}");
                throw;
            }
        }

        private async Task<MigrationResult> MigrateUserData(string sourceUserId, string targetUserId)
        {
            var backupPath = $"{DBConfig.Instance.BackupPrefix}{sourceUserId}_{DateTime.UtcNow}";

            try
            {
                var sourceData = await _cloudRepository.ReadDataAsync<object>(DataType.User, sourceUserId);
                if (sourceData == null)
                    return new MigrationResult { Success = true };

                await _cloudRepository.WriteDataAsync(DataType.User, backupPath, sourceData);

                await _cloudRepository.WriteDataAsync(DataType.User, targetUserId, sourceData);

                var verificationData = await _cloudRepository.ReadDataAsync<object>(DataType.User, targetUserId);
                if (verificationData == null)
                    throw new Exception("Failed to verify migrated data");

                await _cloudRepository.DeleteDataAsync(DataType.User, sourceUserId);

                await _cloudRepository.DeleteDataAsync(DataType.User, backupPath);

                return new MigrationResult { Success = true };
            }
            catch (Exception e)
            {
                await HandleMigrationFailure(sourceUserId, backupPath);

                return new MigrationResult
                {
                    Success = false,
                    Error = $"[UserDataController::ExecuteMigration] Migration failed: {e.Message}"
                };
            }
        }

        private async Task HandleMigrationFailure(string sourceUserId, string backupPath)
        {
            try
            {
                var backupData = await _cloudRepository.ReadDataAsync<object>(DataType.User, backupPath);
                if (backupData != null)
                {
                    await _cloudRepository.WriteDataAsync(DataType.User, sourceUserId, backupData);
                    await _cloudRepository.DeleteDataAsync(DataType.User, backupPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"[UserDataController::HandleMigrationFailure] Failed to restore from backup: {e.Message}");
            }
        }

        private void SaveUserData(UserData data)
        {
            var userDataDTO = data.ToDataTransferObject();
            _offlineRepository.WriteDataAsync(DataType.User, DBConfig.Instance.UserDataPath, userDataDTO);
        }

        public void InjectDependencies()
        {
            DependencyInjector.InjectDependencies(this);
        }
    }

    internal sealed class MigrationResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}