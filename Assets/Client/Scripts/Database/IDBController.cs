using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Scripts.Database
{
    internal interface IDBController
    {
        Task<T> ReadData<T>(string path);
        Task WriteData<T>(string path, T data);
        Task UpdateData(string path, Dictionary<string, object> data);
        Task DeleteData(string path);
    }
}