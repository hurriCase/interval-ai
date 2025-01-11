using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;
using Exception = System.Exception;

namespace Client.Scripts.DB.Entities.CategoryEntity
{
    internal sealed class GlobalCategoryEntity : EntityBase<GlobalCategoryEntryContent>
    {
        protected override string EntityPath => "global_category_entity";

        public override async Task LoadEntryAsync()
        {
            try
            {
                var loadedGlobalCategories = await cloudRepository
                    .LoadDataAsync<Dictionary<string, EntryData<GlobalCategoryEntryContent>>>(
                        DataType.User,
                        GetEntryPath());
                if (loadedGlobalCategories != null)
                {
                    foreach (var (id, categoryData) in loadedGlobalCategories)
                    {
                        Entries[id] = categoryData;

                        cloudRepository.ListenForValueChanged<EntryData<GlobalCategoryEntryContent>>(
                            DataType.User,
                            GetEntryPath(id),
                            _ => categoryData.UpdatedAt = DateTime.Now
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[GlobalCategoryEntity::LoadEntryAsync] Error loading entries: {e.Message}");
            }
        }
    }
}