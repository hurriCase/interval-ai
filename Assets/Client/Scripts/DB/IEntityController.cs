using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Scripts.DB
{
    internal interface IEntityController
    {
        event Action<Type> OnEntityCreated;
        event Action<Type> OnEntityRead;
        event Action<Type> OnEntityUpdated;
        event Action<Type> OnEntityDeleted;
        Task InitAsync();
        Task<bool> ExistsAsync<TData>() where TData : class;
        Task<EntityResult<TData>> CreateEntityAsync<TData>(TData data) where TData : class;
        Task<EntityResult<TData>> ReadEntityAsync<TData>() where TData : class;
        Task<EntityResult<TData>> UpdateEntityAsync<TData>(TData data) where TData : class;
        Task<EntityResult<TData>> DeleteEntityAsync<TData>(string id) where TData : class;
        IEnumerable<TData> FindEntitiesAsync<TData>(Func<TData, bool> predicate) where TData : class;
    }

    internal sealed class EntityResult<TData> where TData : class
    {
        internal bool IsSuccess { get; set; }
        internal TData Data { get; set; }
        internal string ErrorMessage { get; set; }
        internal Exception Exception { get; set; }

        internal static EntityResult<TData> Success(TData data)
        {
            return new EntityResult<TData>
            {
                IsSuccess = true,
                Data = data
            };
        }

        internal static EntityResult<TData> Failure(string message, Exception ex = null)
        {
            return new EntityResult<TData>
            {
                IsSuccess = false,
                ErrorMessage = message,
                Exception = ex
            };
        }
    }
}