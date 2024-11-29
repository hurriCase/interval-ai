using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Scripts.Patterns.DI.Services
{
    internal interface IDBController
    {
        public string UserID { get; set; }
        Task InitAsync();
        Task<T> ReadDataAsync<T>(string path);
        Task WriteDataAsync<T>(string path, T data);
        Task UpdateDataAsync<TData>(string path, Dictionary<string, TData> data);
        Task DeleteDataAsync(string path);
    }
}