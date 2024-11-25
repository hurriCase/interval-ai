using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns;

namespace Client.Scripts.Database
{
    internal interface IDBController
    {
        public IDBController DBReference { get; set; }
        public string UserId { get; set; }
        Task<T> ReadData<T>(string path);
        Task WriteData<T>(string path, T data);
        Task UpdateData(string path, Dictionary<string, object> data);
        Task DeleteData(string path);
    }
}