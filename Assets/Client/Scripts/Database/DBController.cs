using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns;
using Firebase;
using Firebase.Database;
using UnityEngine;

namespace Client.Scripts.Database
{
    [Resource("P_DBController", "DontDestroyOnLoad/P_DBController")]
    internal sealed class DBController : SingletonDontDestroyOnLoad<DBController>
    {
        private DatabaseReference _dbReference;

        internal async Task Init()
        {
            try
            {
                // Make sure all necessary dependencies are available
                await FirebaseApp.CheckAndFixDependenciesAsync();
            
                // Get the root reference location of the database
                _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize Firebase: {e.Message}");
            }
        }

        // Write data to specified path
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

        // Update specific fields at path
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

        // Read data from specified path
        public async Task<T> ReadData<T>(string path)
        {
            try
            {
                var snapshot = await _dbReference.Child(path).GetValueAsync();
                if (snapshot.Exists)
                {
                    // Handle different types of data
                    if (typeof(T) == typeof(string))
                    {
                        // If we're expecting a string, return it directly
                        return (T)(object)snapshot.GetValue(true).ToString();
                    }

                    // For complex objects, use JSON parsing
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

        // Delete data at specified path
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

        // Listen for real-time updates
        public void ListenForValueChanged<T>(string path, Action<T> onValueChanged)
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

        // Stop listening for updates
        public void StopListening(string path)
        {
            _dbReference.Child(path).ValueChanged -= null;
        }
    }
}