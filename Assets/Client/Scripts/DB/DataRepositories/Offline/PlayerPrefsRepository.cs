using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Client.Scripts.Patterns.Singletons;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Scripts.DB.DataRepositories.Offline
{
    internal sealed class PlayerPrefsRepository : Singleton<PlayerPrefsRepository>, IOfflineRepository
    {
        private readonly Dictionary<string, List<Action<object>>> _listeners = new();
        private bool _isInited;

        public Task InitAsync()
        {
            _isInited = true;
            return Task.CompletedTask;
        }

        public Task<T> ReadDataAsync<T>(DataType dataType, string path)
        {
            if (CheckInit() is false)
                return Task.FromResult<T>(default);

            try
            {
                var fullPath = GetFullPath(dataType, path);
                var json = PlayerPrefs.GetString(fullPath);

                if (string.IsNullOrEmpty(json))
                {
                    Debug.Log($"[PlayerPrefsRepository::ReadDataAsync] No data exists at {fullPath}");
                    return Task.FromResult<T>(default);
                }

                var result = JsonConvert.DeserializeObject<T>(json);
                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerPrefsRepository::ReadDataAsync] Error reading data at {path}: {e.Message}");
                return Task.FromResult<T>(default);
            }
        }

        public Task<T> WriteDataAsync<T>(DataType dataType, string path, T data)
        {
            if (CheckInit() is false)
                return Task.FromResult<T>(default);

            try
            {
                var fullPath = GetFullPath(dataType, path);
                var json = JsonConvert.SerializeObject(data);
                PlayerPrefs.SetString(fullPath, json);
                PlayerPrefs.Save();

                NotifyListeners(fullPath, data);
                return Task.FromResult(data);
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerPrefsRepository::WriteDataAsync] Error writing data at {path}: {e.Message}");
                throw;
            }
        }

        public Task UpdateDataAsync<TData>(DataType dataType, string path, TData data) =>

            // In PlayerPrefs, Update is same as Write since we're overwriting the value
            WriteDataAsync(dataType, path, data);

        public Task DeleteDataAsync(DataType dataType, string path)
        {
            if (CheckInit() is false)
                return Task.CompletedTask;

            try
            {
                var fullPath = GetFullPath(dataType, path);
                PlayerPrefs.DeleteKey(fullPath);
                PlayerPrefs.Save();

                NotifyListeners(fullPath, null);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerPrefsRepository::DeleteDataAsync] Error deleting data at {path}: {e.Message}");
                throw;
            }
        }

        public void ListenForValueChanged<T>(DataType dataType, string path, Action<T> onValueChanged)
        {
            if (CheckInit() is false)
                return;

            var fullPath = GetFullPath(dataType, path);
            if (_listeners.ContainsKey(fullPath) is false)
                _listeners[fullPath] = new List<Action<object>>();

            _listeners[fullPath].Add(value => onValueChanged((T)value));
        }

        public void StopListening(DataType dataType, string path)
        {
            if (CheckInit() is false)
                return;

            var fullPath = GetFullPath(dataType, path);
            if (_listeners.ContainsKey(fullPath))
                _listeners.Remove(fullPath);
        }

        private bool CheckInit()
        {
            if (_isInited is false)
                Debug.LogError("[PlayerPrefsRepository::CheckInit] Not initialized but trying to access");

            return _isInited;
        }

        private string GetFullPath(DataType dataType, string path) =>
            dataType switch
            {
                DataType.User => $"{DBConfig.Instance.UserPath}/{UserData.Instance.UserID}/{path}",
                DataType.Configs => $"{DBConfig.Instance.ConfigsPath}/{path}",
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
            };

        private void NotifyListeners(string path, object value)
        {
            if (_listeners.TryGetValue(path, out var listeners))
                foreach (var listener in listeners)
                    listener?.Invoke(value);
        }
    }
}