using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Client.Scripts.Patterns.Singletons;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Scripts.DB.DataRepositories.Cloud
{
    internal sealed class FireBaseRepository : Singleton<FireBaseRepository>, ICloudRepository
    {
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
                            "[FireBaseDB::InitAsync] " +
                            $"Could not resolve all Firebase dependencies: {dependencyStatus}");
                });

                _isInited = true;

                Debug.Log("[FireBaseDB::InitAsync] FirebaseDatabase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[FireBaseDB::InitAsync] Failed to initialize FirebaseDatabase: {e.Message}");
            }
        }

        public async Task<TData> LoadDataAsync<TData>(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return default;

            try
            {
                var snapshot = await GetDBPath(dataType, path).GetValueAsync();
                if (snapshot.Exists is false)
                {
                    Debug.Log($"[FireBaseDB::LoadDataAsync] No data exists at {path}");
                    return default;
                }

                var json = snapshot.GetRawJsonValue();

                //TODO: Refactor
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
                Debug.LogError($"[FireBaseRepository::LoadDataAsync] Error loading data at {path}: {e.Message}");
                return default;
            }
        }

        public async Task<TData> WriteDataAsync<TData>(DataType dataType, string path, TData data)
        {
            if (CheckDBInit() is false)
                return default;

            var dataToWrite = "";
            try
            {
                //TODO: Refactor
                if (data is string str)
                {
                    dataToWrite = str;
                    await GetDBPath(dataType, path).SetValueAsync(data);
                }
                else
                {
                    dataToWrite = JsonConvert.SerializeObject(data);

                    await GetDBPath(dataType, path).SetRawJsonValueAsync(dataToWrite);
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

        public async Task UpdateDataAsync<TData>(DataType dataType, string path, TData data)
        {
            if (CheckDBInit() is false)
                return;

            try
            {
                if (data is string str)
                    await GetDBPath(dataType, path).SetValueAsync(str);
                else
                {
                    var json = JsonConvert.SerializeObject(data);
                    await GetDBPath(dataType, path).SetRawJsonValueAsync(json);
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

        public async Task<TData> ReadDataAsync<TData>(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return default;

            try
            {
                var snapshot = await GetDBPath(dataType, path).GetValueAsync();
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

        public async Task DeleteDataAsync(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return;

            try
            {
                StopListening(dataType, path);
                await GetDBPath(dataType, path).RemoveValueAsync();
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

        public void ListenForValueChanged<TData>(DataType dataType, string path, Action<TData> onValueChanged)
        {
            if (CheckDBInit() is false)
                return;

            if (_isInited is false)
            {
                Debug.LogError("[FireBaseDB::WriteDataAsync] DBController not initialized!");
                return;
            }

            GetDBPath(dataType, path).ValueChanged += (_, args) =>
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

        public void StopListening(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return;

            GetDBPath(dataType, path).ValueChanged -= null;
        }

        private bool CheckDBInit()
        {
            if (_isInited is false)
                Debug.LogError("[FireBaseDB::CheckDBInit] FireBaseDB not initialized but you're trying to access");

            return _isInited;
        }

        private DatabaseReference GetDBPath(DataType dataType, string path)
        {
            switch (dataType)
            {
                case DataType.User:
                    var userPath = DBConfig.Instance.UserPath;
                    var userID = UserData.Instance.UserID;
                    return _dbReference.Child(userPath).Child(userID).Child(path);

                case DataType.Configs:
                    var configsPath = DBConfig.Instance.ConfigsPath;
                    return _dbReference.Child(configsPath).Child(path);

                case DataType.Tests:
                    var testsPath = DBConfig.Instance.TestsPath;
                    return _dbReference.Child(testsPath).Child(path);

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }
    }
}