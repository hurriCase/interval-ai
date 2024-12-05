using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Client.Scripts.DB.Entities.Base
{
    internal interface IEntity<TContent> where TContent : class
    {
        ConcurrentDictionary<string, EntryData<TContent>> Entries { get; set; }
        Task LoadEntryAsync();
        Task<EntryData<TContent>> CreateEntryAsync(TContent content);
        Task<EntryData<TContent>> ReadEntryAsync(string id);
        Task<EntryData<TContent>> UpdateEntryAsync(EntryData<TContent> entry);
        Task<EntryData<TContent>> DeleteEntryAsync(EntryData<TContent> entry);
        Task InitAsync();
    }
}