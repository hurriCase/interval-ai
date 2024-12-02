using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Base;
using UnityEngine;

namespace Client.Scripts.DB.Entities
{
    internal sealed class CategoryEntity : DBEntityBase<CategoryEntityData>
    {
        public override async Task InitAsync()
        {
            try
            {
                var customCategory =
                    await dbController.ReadDataAsync<ConcurrentDictionary<string, EntityData<CategoryEntityData>>>(
                        GetPath());
                if (customCategory != null)
                    Entities = customCategory;

                var globalCategory =
                    await dbController
                        .ReadDataAsync<ConcurrentDictionary<string, EntityData<CategoryEntityData>>>(
                            "global_categories");
                if (globalCategory != null)
                    Entities = globalCategory;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CategoryEntity::LoadEntityAsync] Error loading entities: {e.Message}");
            }
        }

        protected override string GetPath() => "user_categories";
    }

    [Serializable]
    internal struct CategoryEntityData
    {
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public string Description { get; set; }
        [field: SerializeField] public List<EntityData<WordEntityData>> Words { get; set; }
        [field: SerializeField] public bool IsDefault { get; set; }
    }
}