using System;
using System.Threading.Tasks;

namespace Client.Scripts.Patterns.DI.Services
{
    internal interface ICloudRepository
    {
        Task InitAsync();
        Task<TData> LoadDataAsync<TData>(DataType dataType, string path);
        Task<TData> ReadDataAsync<TData>(DataType dataType, string path);
        Task<TData> WriteDataAsync<TData>(DataType dataType, string path, TData data);
        Task UpdateDataAsync<TData>(DataType dataType, string path, TData data);
        Task DeleteDataAsync(DataType dataType, string path);
        void ListenForValueChanged<TData>(DataType dataType, string path, Action<TData> onValueChanged);
        void StopListening(DataType dataType, string path);
    }
}