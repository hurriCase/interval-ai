using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Scripts.Patterns.DI.Services
{
    internal interface IDBController
    {
        public string UserID { get; set; }
        Task Init();
        Task<T> ReadData<T>(string path);
        Task WriteData<T>(string path, T data);
        Task UpdateData(string path, Dictionary<string, object> data);
        Task DeleteData(string path);
    }
}