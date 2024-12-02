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
        Task<bool> ExistsAsync<TData>() where TData : struct;
        Task<EntityResult<TData>> CreateEntityAsync<TData>(TData data) where TData : struct;
        Task<EntityResult<TData>> ReadEntityAsync<TData>() where TData : struct;
        Task<EntityResult<TData>> UpdateEntityAsync<TData>(TData data) where TData : struct;
        Task<EntityResult<TData>> DeleteEntityAsync<TData>(string id) where TData : struct;
        IEnumerable<TData> FindEntitiesAsync<TData>(Func<TData, bool> predicate) where TData : struct;
    }

    internal struct EntityResult<TData> where TData : struct
    {
        internal bool IsSuccess { get; private set; }
        internal TData Data { get; private set; }
        internal string ErrorMessage { get; private set; }
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