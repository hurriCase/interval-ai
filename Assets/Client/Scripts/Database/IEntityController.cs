using System.Threading.Tasks;
using Client.Scripts.Database.Base;

namespace Client.Scripts.Database
{
    internal interface IEntityController
    {
        Task InitAsync();

        public IEntity<TData> GetEntity<TEntity, TData>()
            where TEntity : IEntity<TData>
            where TData : class;

        public EntityData<TData> GetEntityData<TData>()
            where TData : class;

        public Task<EntityData<TSpecificData>> CreateEntity<TEntity, TSpecificData>(TEntity entity,
            TSpecificData entityData)
            where TSpecificData : class, new()
            where TEntity : IEntity<TSpecificData>;

        public Task<EntityData<TData>> ReadEntity<TEntity, TData>(IEntity<TData> typedEntity)
            where TEntity : class
            where TData : class;

        public Task<EntityData<TData>> UpdateEntity<TData>(IEntity<TData> entity, EntityData<TData> entityData)
            where TData : class;

        public Task<EntityData<TData>> DeleteEntity<TData>(IEntity<TData> entity, EntityData<TData> entityData)
            where TData : class;
    }
}