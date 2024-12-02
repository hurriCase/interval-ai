using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Services;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Client.Scripts.DB.Base
{
    internal sealed class DBController : IDBController
    {
        public string UserID { get; set; }
        private DatabaseReference _dbReference;
        private bool _isInited;

        public async Task InitAsync()
        {
            if (_isInited)
                return;

            try
            {
                await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == DependencyStatus.Available)
                        _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                    else
                        Debug.LogError(
                            "[DBController::InitAsync] " +
                            $"Could not resolve all Firebase dependencies: {dependencyStatus}");
                });

                _isInited = true;
                Debug.Log("[DBController::InitAsync] FirebaseDatabase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::InitAsync] Failed to initialize FirebaseDatabase: {e.Message}");
            }
        }

        public async Task<T> WriteDataAsync<T>(string path, T data)
        {
            if (ValidateDB() is false)
                return default;

            var dataToWrite = "";
            try
            {
                if (data is string)
                    await GetDBPath(path).SetValueAsync(data);
                else
                {
                    dataToWrite = JsonUtility.ToJson(data);
                    await GetDBPath(path).SetRawJsonValueAsync(dataToWrite);
                }

                Debug.Log($"[DBController::WriteDataAsync] Data written successfully to {path}");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::WriteDataAsync] Error writing data: {dataToWrite} " +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public async Task UpdateDataAsync<TData>(string path, ConcurrentDictionary<string, TData> data)
        {
            if (ValidateDB() is false)
                return;

            try
            {
                //TODO:<dmitriy.sukharev> Test this
                await GetDBPath(path).UpdateChildrenAsync((IDictionary<string, object>)data);
                Debug.Log($"[DBController::UpdateDataAsync] Data updated successfully at {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::UpdateDataAsync] Error updating data: {data} " +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public async Task<T> ReadDataAsync<T>(string path)
        {
            if (ValidateDB() is false)
                return default;

            try
            {
                var snapshot = await GetDBPath(path).GetValueAsync();
                if (snapshot.Exists)
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)snapshot.GetValue(true).ToString();
                    ;

                    var json = snapshot.GetRawJsonValue();
                    return JsonUtility.FromJson<T>(json);
                    ;
                }

                Debug.Log($"[DBController::ReadDataAsync] No data exists at {path}");
                return default;
            }
            catch (Exception e)
            {
                Debug.LogError("[DBController::ReadDataAsync] Error reading data:" +
                               $"for Entity type: {typeof(T)} " +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public async Task DeleteDataAsync(string path)
        {
            if (ValidateDB() is false)
                return;

            if (_isInited is false)
            {
                Debug.LogError("[DBController::WriteDataAsync] DBController not initialized!");
                return;
            }

            try
            {
                await GetDBPath(path).RemoveValueAsync();
                Debug.Log($"[DBController::DeleteDataAsync] Data deleted successfully from {path}");
            }
            catch (Exception e)
            {
                Debug.LogError("[DBController::DeleteDataAsync] Error deleting data" +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public void ListenForValueChanged<T>(string path, Action<T> onValueChanged)
        {
            if (ValidateDB() is false)
                return;

            if (_isInited is false)
            {
                Debug.LogError("[DBController::WriteDataAsync] DBController not initialized!");
                return;
            }

            GetDBPath(path).ValueChanged += (_, args) =>
            {
                if (args.DatabaseError != null)
                {
                    Debug.LogError("[DBController::ListenForValueChanged] " +
                                   $"Error listening to data: {args.DatabaseError.Message}");
                    return;
                }

                if (args.Snapshot == null || args.Snapshot.Exists is false) return;

                var json = args.Snapshot.GetRawJsonValue();
                var value = JsonUtility.FromJson<T>(json);
                onValueChanged?.Invoke(value);
            };
        }

        public void StopListening(string path)
        {
            if (ValidateDB() is false)
                return;

            GetDBPath(path).ValueChanged -= null;
        }

        private bool ValidateDB()
        {
            if (_isInited is false)
                Debug.LogError("[DBController::ValidateDB] DBController not initialized but you're trying to access");

            return _isInited;
        }

        private DatabaseReference GetDBPath(string path) => _dbReference.Child(UserID).Child(path);
    }
}