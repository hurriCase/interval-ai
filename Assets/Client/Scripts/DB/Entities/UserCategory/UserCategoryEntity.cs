using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.Entities.Base;
using UnityEngine;
using Exception = System.Exception;

namespace Client.Scripts.DB.Entities.UserCategory
{
    internal sealed class UserCategoryEntity : EntityBase<UserCategoryEntryContent>
    {
        protected override string EntityPath => "user_category_entity";

        public override async Task LoadEntryAsync()
        {
            try
            {
                var loadedCustomCategories = await cloudRepository
                    .LoadDataAsync<Dictionary<string, EntryData<UserCategoryEntryContent>>>(DataType.User,
                        GetEntryPath());
                if (loadedCustomCategories != null)
                {
                    Entries.Clear();

                    foreach (var (id, categoryData) in loadedCustomCategories)
                    {
                        Entries[id] = categoryData;
                        cloudRepository.ListenForValueChanged<EntryData<UserCategoryEntryContent>>(
                            DataType.User,
                            GetEntryPath(id),
                            _ => categoryData.UpdatedAt = DateTime.Now
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[CategoryEntity::LoadEntryAsync] Error loading entries: {e.Message}");
            }
        }
    }
}