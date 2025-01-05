using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Services;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Scripts.DB.DBControllers
{
    internal sealed class FireBaseDB : IDBController
    {
        private const string UserFolderName = "users";

        private DatabaseReference _dbReference;
        private bool _isInited;

        public string UserID { get; set; }

        public async Task InitAsync(string userID)
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
                            "[FireBaseDB::InitAsync] " +
                            $"Could not resolve all Firebase dependencies: {dependencyStatus}");
                });

                UserID = userID;

                _isInited = true;

                Debug.Log("[FireBaseDB::InitAsync] FirebaseDatabase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[FireBaseDB::InitAsync] Failed to initialize FirebaseDatabase: {e.Message}");
            }
        }

        public async Task<TData> WriteDataAsync<TData>(string path, TData data)
        {
            if (CheckDBInit() is false)
                return default;

            var dataToWrite = "";
            try
            {
                if (data is string str)
                {
                    dataToWrite = str;
                    await GetDBPath(path).SetValueAsync(data);
                }
                else
                {
                    dataToWrite = JsonConvert.SerializeObject(data);
                    await GetDBPath(path).SetRawJsonValueAsync(dataToWrite);
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[FireBaseDB::WriteDataAsync] Error writing data: {dataToWrite} " +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public async Task UpdateDataAsync<TData>(string path, TData data)
        {
            if (CheckDBInit() is false)
                return;

            try
            {
                if (data is string str)
                    await GetDBPath(path).SetValueAsync(str);
                else
                {
                    var json = JsonConvert.SerializeObject(data);
                    await GetDBPath(path).SetRawJsonValueAsync(json);
                }

                Debug.Log($"[FireBaseDB::UpdateDataAsync] Data updated successfully at {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[FireBaseDB::UpdateDataAsync] Error updating data: {data} " +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public async Task<TData> ReadDataAsync<TData>(string path)
        {
            if (CheckDBInit() is false)
                return default;

            try
            {
                var snapshot = await GetDBPath(path).GetValueAsync();
                if (snapshot.Exists is false)
                {
                    Debug.Log($"[FireBaseDB::ReadDataAsync] No data exists at {path}");
                    return default;
                }

                var json = snapshot.GetRawJsonValue();

                if (typeof(TData) == typeof(Dictionary<,>))
                {
                    var keyType = typeof(TData).GetGenericArguments()[0];
                    var valueType = typeof(TData).GetGenericArguments()[1];
                    var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

                    var dictionary = Activator.CreateInstance(dictionaryType);
                    var data = JsonConvert.DeserializeObject(json, valueType);

                    dictionaryType.GetMethod("Add")?.Invoke(dictionary, new[] { snapshot.Key, data });

                    return (TData)(object)dictionaryType;
                }

                return JsonConvert.DeserializeObject<TData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("[FireBaseDB::ReadDataAsync] Error reading data:" +
                               $"for Entity type: {typeof(TData)} " +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                return default;
            }
        }

        public async Task DeleteDataAsync(string path)
        {
            if (CheckDBInit() is false)
                return;

            try
            {
                StopListening(path);
                await GetDBPath(path).RemoveValueAsync();
                Debug.Log($"[FireBaseDB::DeleteDataAsync] Data deleted successfully from {path}");
            }
            catch (Exception e)
            {
                Debug.LogError("[FireBaseDB::DeleteDataAsync] Error deleting data" +
                               $"at path {path} " +
                               $"with error: {e.Message}");
                throw;
            }
        }

        public void ListenForValueChanged<TData>(string path, Action<TData> onValueChanged)
        {
            if (CheckDBInit() is false)
                return;

            if (_isInited is false)
            {
                Debug.LogError("[FireBaseDB::WriteDataAsync] DBController not initialized!");
                return;
            }

            GetDBPath(path).ValueChanged += (_, args) =>
            {
                if (args.DatabaseError != null)
                {
                    Debug.LogError("[FireBaseDB::ListenForValueChanged] " +
                                   $"Error listening to data: {args.DatabaseError.Message}");
                    return;
                }

                try
                {
                    var json = args.Snapshot.GetRawJsonValue();
                    if (json == null)
                        return;

                    var value = JsonConvert.DeserializeObject<TData>(json);
                    onValueChanged?.Invoke(value);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[FireBaseDB::ListenForValueChanged] Deserialization error: {e.Message}");
                }
            };
        }

        public void StopListening(string path)
        {
            if (CheckDBInit() is false)
                return;

            GetDBPath(path).ValueChanged -= null;
        }

        private bool CheckDBInit()
        {
            if (_isInited is false)
                Debug.LogError("[FireBaseDB::CheckDBInit] FireBaseDB not initialized but you're trying to access");

            return _isInited;
        }

        private DatabaseReference GetDBPath(string path) =>
            _dbReference.Child(UserFolderName).Child(UserID).Child(path);
    }
}