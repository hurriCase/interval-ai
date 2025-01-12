using System;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.EntityController;

namespace Client.Scripts.Core.MVC.Base
{
    internal abstract class ControllerBase<TEntity, TContent, TModel>
        where TEntity : IEntity<TContent>
        where TContent : class
        where TModel : ModelBase<TContent>
    {
        private readonly IEntityController _entityController;
        private readonly IView<TModel> _view;

        internal async Task<bool> UpdateEntry(string id, TContent content)
        {
            var readResult = await _entityController.ReadEntryAsync<TEntity, TContent>(id);
            if (!readResult.IsSuccess) return false;

            var updateResult =
                await _entityController.UpdateEntryAsync<TEntity, TContent>(readResult.EntryData, content);
            return updateResult.IsSuccess;
        }

        internal async Task<bool> DeleteEntry(string id)
        {
            var result = await _entityController.DeleteEntryAsync<TEntity, TContent>(id);
            return result.IsSuccess;
        }

        protected ControllerBase(IEntityController entityController, IView<TModel> view)
        {
            _entityController = entityController;
            _view = view;

            entityController.OnEntryCreated += OnEntryChanged;
            entityController.OnEntryUpdated += OnEntryChanged;
            entityController.OnEntryDeleted += OnEntryChanged;
        }

        protected async Task<bool> CreateEntry(TContent content)
        {
            var result = await _entityController.CreateEntryAsync<TEntity, TContent>(content);
            return result.IsSuccess;
        }

        protected abstract TModel CreateModel(EntryData<TContent> data);

        private void OnEntryChanged(Type type, object content)
        {
            if (type != typeof(TContent)) return;
            RefreshView();
        }

        private void RefreshView()
        {
            _view.ClearView();
            var entries = _entityController.FindEntries<TEntity, TContent>(_ => true);

            if (entries == null) return;

            foreach (var entry in entries)
            {
                var model = CreateModel(entry);
                _view.UpdateView(model);
            }
        }
    }
}