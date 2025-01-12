using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Scripts.DB.DataRepositories.Cloud
{
    //TODO: Fix nested data saving, currently this db doesn't work
    internal sealed class FirestoreRepository : ICloudRepository
    {
        private FirebaseFirestore _firestoreInstance;
        private bool _isInited;
        private readonly Dictionary<string, ListenerRegistration> _listeners = new();

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
                    {
                        _firestoreInstance = FirebaseFirestore.DefaultInstance;
                        _firestoreInstance.Settings.PersistenceEnabled = true;
                        _firestoreInstance.Settings.CacheSizeBytes = FirebaseFirestoreSettings.CacheSizeUnlimited;
                    }
                    else
                        Debug.LogError(
                            $"[FirestoreRepository::InitAsync] Could not resolve Firebase dependencies: {dependencyStatus}");
                });

                _isInited = true;
                Debug.Log("[FirestoreRepository::InitAsync] Firestore initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[FirestoreRepository::InitAsync] Failed to initialize Firestore: {e.Message}");
                throw;
            }
        }

        public async Task<TData> LoadDataAsync<TData>(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return default;

            try
            {
                var collectionRef = _firestoreInstance
                    .Collection(DBConfig.Instance.UserPath)
                    .Document(UserData.Instance.UserID)
                    .Collection(path);

                var snapshots = await collectionRef.GetSnapshotAsync();

                if (snapshots is null || snapshots.Count == 0)
                {
                    Debug.Log($"[FirestoreRepository::LoadDataAsync] No data exists at {path}");
                    return default;
                }

                var result = new Dictionary<string, object>();
                foreach (var doc in snapshots.Documents)
                {
                    if (doc.Exists)
                        result[doc.Id] = doc.ConvertTo<Dictionary<string, object>>();
                }

                var json = JsonConvert.SerializeObject(result);
                return JsonConvert.DeserializeObject<TData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[FirestoreRepository::LoadDataAsync] Error loading data at {path}: {e.Message}");
                return default;
            }
        }

        public async Task<TData> ReadDataAsync<TData>(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return default;

            try
            {
                var docRef = GetDocumentReference(dataType, path);
                var snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists is false)
                {
                    Debug.Log($"[FirestoreRepository::ReadDataAsync] No data exists at {path}");
                    return default;
                }

                var dictionary = snapshot.ConvertTo<Dictionary<string, object>>();
                var json = JsonConvert.SerializeObject(dictionary);
                return JsonConvert.DeserializeObject<TData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[FirestoreRepository::ReadDataAsync] Error reading data at {path}: {e.Message}");
                return default;
            }
        }

        public async Task<TData> WriteDataAsync<TData>(DataType dataType, string path, TData data)
        {
            if (CheckDBInit() is false)
                return default;

            try
            {
                var docRef = GetDocumentReference(dataType, path);
                await docRef.SetAsync(FirestoreUtils.ToFirestoreDictionary(data));
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[FirestoreRepository::WriteDataAsync] Error writing data at {path}: {e.Message}");
                throw;
            }
        }

        public async Task UpdateDataAsync<TData>(DataType dataType, string path, TData data)
        {
            if (CheckDBInit() is false)
                return;

            try
            {
                var docRef = GetDocumentReference(dataType, path);

                var serializedData = JsonConvert.SerializeObject(data);
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedData);

                await docRef.UpdateAsync(dictionary);
            }
            catch (Exception e)
            {
                Debug.LogError($"[FirestoreRepository::UpdateDataAsync] Error updating data at {path}: {e.Message}");
                throw;
            }
        }

        public async Task DeleteDataAsync(DataType dataType, string path)
        {
            if (CheckDBInit() is false)
                return;

            try
            {
                StopListening(dataType, path);
                var docRef = GetDocumentReference(dataType, path);

                await docRef.DeleteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"[FirestoreRepository::DeleteDataAsync] Error deleting data at {path}: {e.Message}");
                throw;
            }
        }

        public void ListenForValueChanged<TData>(DataType dataType, string path, Action<TData> onValueChanged)
        {
            if (CheckDBInit() is false)
                return;

            var docRef = GetDocumentReference(dataType, path);
            var listenerKey = $"{dataType}:{path}";

            if (_listeners.ContainsKey(listenerKey))
                StopListening(dataType, path);

            var registration = docRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                    try
                    {
                        var dictionary = snapshot.ConvertTo<Dictionary<string, object>>();
                        var json = JsonConvert.SerializeObject(dictionary);
                        var value = JsonConvert.DeserializeObject<TData>(json);
                        onValueChanged?.Invoke(value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(
                            $"[FirestoreRepository::ListenForValueChanged] Deserialization error: {e.Message}");
                    }
            });

            _listeners[listenerKey] = registration;
        }

        public void StopListening(DataType dataType, string path)
        {
            var listenerKey = $"{dataType}:{path}";
            if (_listeners.TryGetValue(listenerKey, out var registration))
            {
                registration.Stop();
                _listeners.Remove(listenerKey);
            }
        }

        private bool CheckDBInit()
        {
            if (_isInited is false)
                Debug.LogError("[FirestoreRepository::CheckDBInit] Firestore not initialized but trying to access");

            return _isInited;
        }

        private DocumentReference GetDocumentReference(DataType dataType, string path)
        {
            Debug.LogWarning($"[FirestoreRepository::GetDocumentReference] path{path}");
            switch (dataType)
            {
                //TODO: Refactor
                case DataType.User:
                    var splitPath = path.Split("/");
                    var isCollection = splitPath.Length > 1;
                    return _firestoreInstance.Collection(DBConfig.Instance.UserPath)
                        .Document(UserData.Instance.UserID)
                        .Collection(splitPath[0])
                        .Document(isCollection ? splitPath[1] : splitPath[0]);

                case DataType.Configs:
                    return _firestoreInstance.Collection(DBConfig.Instance.ConfigsPath).Document(path);

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }
    }
}