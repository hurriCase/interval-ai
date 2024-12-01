using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Services;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Client.Scripts.Database.Base
{
    internal sealed class DBController : IDBController
    {
        public string UserID { get; set; }
        private DatabaseReference _dbReference;

        public async Task InitAsync()
        {
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

                Debug.Log($"[DBController::InitAsync] FirebaseDatabase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::InitAsync] Failed to initialize FirebaseDatabase: {e.Message}");
            }
        }

        public async Task WriteDataAsync<T>(string path, T data)
        {
            try
            {
                var dataToWrite = data as string ?? JsonUtility.ToJson(data);
                await GetDBPath(path).SetRawJsonValueAsync(dataToWrite);
                Debug.Log($"[DBController::WriteDataAsync] Data written successfully to {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::WriteDataAsync] Error writing data: {e.Message}");
                throw;
            }
        }

        public async Task UpdateDataAsync<TData>(string path, ConcurrentDictionary<string, TData> data)
        {
            try
            {
                //TODO:<dmitriy.sukharev> Test this
                await GetDBPath(path).UpdateChildrenAsync((IDictionary<string, object>)data);
                Debug.Log($"[DBController::UpdateDataAsync] Data updated successfully at {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::UpdateDataAsync] Error updating data: {e.Message}");
                throw;
            }
        }

        public async Task<T> ReadDataAsync<T>(string path)
        {
            try
            {
                var snapshot = await GetDBPath(path).GetValueAsync();
                if (snapshot.Exists)
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)snapshot.GetValue(true).ToString();

                    var json = snapshot.GetRawJsonValue();
                    return JsonUtility.FromJson<T>(json);
                }

                Debug.Log($"[DBController::ReadDataAsync] No data exists at {path}");
                return default;
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::ReadDataAsync] Error reading data: {e.Message}");
                throw;
            }
        }

        public async Task DeleteDataAsync(string path)
        {
            try
            {
                await GetDBPath(path).RemoveValueAsync();
                Debug.Log($"[DBController::DeleteDataAsync] Data deleted successfully from {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBController::DeleteDataAsync] Error deleting data: {e.Message}");
                throw;
            }
        }

        public void ListenForValueChanged<T>(string path, Action<T> onValueChanged)
        {
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

        public void StopListening(string path) => GetDBPath(path).ValueChanged -= null;

        private DatabaseReference GetDBPath(string path) => _dbReference.Child(UserID).Child(path);
    }
}