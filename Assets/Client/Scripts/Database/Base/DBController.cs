using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using Firebase;
using Firebase.Database;
using UnityEngine;

namespace Client.Scripts.Database.Base
{
    internal sealed class DBController : IDBController
    {
        private DatabaseReference _dbReference;
        public string UserID { get; set; }

        public async Task Init()
        {
            try
            {
                await FirebaseApp.CheckAndFixDependenciesAsync();

                _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize Firebase: {e.Message}");
            }
        }

        public async Task WriteData<T>(string path, T data)
        {
            try
            {
                var dataToWrite = data as string ?? JsonUtility.ToJson(data);
                await _dbReference.Child(path).SetRawJsonValueAsync(dataToWrite);
                Debug.Log($"Data written successfully to {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error writing data: {e.Message}");
                throw;
            }
        }

        public async Task UpdateData(string path, Dictionary<string, object> data)
        {
            try
            {
                await _dbReference.Child(path).UpdateChildrenAsync(data);
                Debug.Log($"Data updated successfully at {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating data: {e.Message}");
                throw;
            }
        }

        public async Task<T> ReadData<T>(string path)
        {
            try
            {
                var snapshot = await _dbReference.Child(path).GetValueAsync();
                if (snapshot.Exists)
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)snapshot.GetValue(true).ToString();

                    var json = snapshot.GetRawJsonValue();
                    return JsonUtility.FromJson<T>(json);
                }

                Debug.Log($"No data exists at {path}");
                return default;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading data: {e.Message}");
                throw;
            }
        }

        public async Task DeleteData(string path)
        {
            try
            {
                await _dbReference.Child(path).RemoveValueAsync();
                Debug.Log($"Data deleted successfully from {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting data: {e.Message}");
                throw;
            }
        }

        internal void ListenForValueChanged<T>(string path, Action<T> onValueChanged)
        {
            _dbReference.Child(path).ValueChanged += (_, args) =>
            {
                if (args.DatabaseError != null)
                {
                    Debug.LogError($"Error listening to data: {args.DatabaseError.Message}");
                    return;
                }

                if (args.Snapshot == null || args.Snapshot.Exists is false) return;

                var json = args.Snapshot.GetRawJsonValue();
                var value = JsonUtility.FromJson<T>(json);
                onValueChanged?.Invoke(value);
            };
        }

        internal void StopListening(string path) => _dbReference.Child(path).ValueChanged -= null;
    }
}