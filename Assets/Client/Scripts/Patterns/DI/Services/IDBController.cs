using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Client.Scripts.Patterns.DI.Services
{
    internal interface IDBController
    {
        public string UserID { get; set; }
        Task InitAsync();
        Task<T> ReadDataAsync<T>(string path);
        Task<T> WriteDataAsync<T>(string path, T data);
        Task UpdateDataAsync<TData>(string path, ConcurrentDictionary<string, TData> data);
        Task DeleteDataAsync(string path);
        void ListenForValueChanged<T>(string path, Action<T> onValueChanged);
        void StopListening(string path);
    }
}