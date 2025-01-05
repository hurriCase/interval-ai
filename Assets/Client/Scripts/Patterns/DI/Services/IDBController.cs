using System;
using System.Threading.Tasks;

namespace Client.Scripts.Patterns.DI.Services
{
    internal interface IDBController
    {
        Task InitAsync(string userId);
        Task<T> ReadDataAsync<T>(string path);
        Task<T> WriteDataAsync<T>(string path, T data);
        Task UpdateDataAsync<TData>(string path, TData data);
        Task DeleteDataAsync(string path);
        void ListenForValueChanged<T>(string path, Action<T> onValueChanged);
        void StopListening(string path);
    }
}