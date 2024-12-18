using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;
using UnityEngine;
using Exception = System.Exception;

namespace Client.Scripts.DB.Entities.CategoryEntity
{
    internal sealed class CategoryEntity : EntityBase<CategoryEntryContent>
    {
        protected override string EntityPath => "category_entity";

        public override async Task LoadEntryAsync()
        {
            try
            {
                var loadedCustomCategories = await dbController
                    .ReadDataAsync<Dictionary<string, EntryData<CategoryEntryContent>>>(GetEntryPath(string.Empty));
                if (loadedCustomCategories != null)
                {
                    Entries.Clear();

                    foreach (var (id, categoryData) in loadedCustomCategories)
                    {
                        Entries[id] = categoryData;
                        dbController.ListenForValueChanged<EntryData<CategoryEntryContent>>(
                            GetEntryPath(id),
                            _ => categoryData.UpdatedAt = DateTime.Now
                        );
                    }
                }

                var loadedGlobalCategories = await dbController
                    .ReadDataAsync<Dictionary<string, EntryData<CategoryEntryContent>>>(
                        GetEntryPath("global_categories"));
                if (loadedGlobalCategories != null)
                {
                    foreach (var (id, categoryData) in loadedGlobalCategories)
                    {
                        Entries[id] = categoryData;

                        dbController.ListenForValueChanged<EntryData<CategoryEntryContent>>(
                            GetEntryPath(id),
                            _ => categoryData.UpdatedAt = DateTime.Now
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[EntityBase::LoadEntryAsync] Error loading entries: {e.Message}");
            }
        }
    }
}