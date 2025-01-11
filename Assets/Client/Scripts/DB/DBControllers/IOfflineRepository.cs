using System;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Services;

namespace Client.Scripts.DB.DBControllers
{
    internal interface IOfflineRepository
    {
        Task InitAsync();
        Task<T> ReadDataAsync<T>(DataType dataType, string path);
        Task<T> WriteDataAsync<T>(DataType dataType, string path, T data);
        Task UpdateDataAsync<TData>(DataType dataType, string path, TData data);
        Task DeleteDataAsync(DataType dataType, string path);
        void ListenForValueChanged<T>(DataType dataType, string path, Action<T> onValueChanged);
        void StopListening(DataType dataType, string path);
    }
}