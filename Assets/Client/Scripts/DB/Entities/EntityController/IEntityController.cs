using System;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;

namespace Client.Scripts.DB.Entities.EntityController
{
    internal interface IEntityController
    {
        event Action<Type> OnEntryCreated;
        event Action<Type> OnEntryRead;
        event Action<Type> OnEntryUpdated;
        event Action<Type> OnEntryDeleted;
        Task InitAsync();

        Task<EntityResult<TContent>> CreateEntryAsync<TEntity, TContent>(TContent content)
            where TEntity : IEntity<TContent>
            where TContent : class;

        Task<EntityResult<TContent>> ReadEntryAsync<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class;

        Task<EntityResult<TContent>> UpdateEntryAsync<TEntity, TContent>
            (EntryData<TContent> entry, TContent content)
            where TEntity : IEntity<TContent>
            where TContent : class;

        Task<EntityResult<TContent>> DeleteEntryAsync<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class;

        EntryData<TContent>[] FindEntriesAsync<TEntity, TContent>
            (Func<TContent, bool> predicate)
            where TEntity : IEntity<TContent>
            where TContent : class;

        Task<bool> ExistsAsync<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class;
    }
}