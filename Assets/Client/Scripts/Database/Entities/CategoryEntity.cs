using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Database.Base;
using UnityEngine;

namespace Client.Scripts.Database.Entities
{
    internal sealed class CategoryEntity : DBEntityBase<CategoryEntityData>
    {
        public override async Task LoadEntityAsync()
        {
            try
            {
                var customCategory =
                    await dbController.ReadDataAsync<Dictionary<string, EntityData<CategoryEntityData>>>(GetPath());
                if (customCategory != null)
                    Entities = customCategory;

                var globalCategory =
                    await dbController
                        .ReadDataAsync<Dictionary<string, EntityData<CategoryEntityData>>>("global_categories");
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

    internal sealed class CategoryEntityData
    {
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal List<EntityData<WordEntityData>> Words { get; set; } = new();
        internal bool IsDefault { get; set; }
    }
}