using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Client.Scripts.Database.Vocabulary
{
    internal sealed class CategoryEntityData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EntityData<WordEntityData>> Words { get; set; } = new();
        public bool IsDefault { get; set; }
    }

    internal sealed class CategoryEntity : DataBaseEntity<CategoryEntityData>, IInitializable
    {
        protected override string GetPath(string userId) => $"user_categories/{userId}";


        internal void AddWord(EntityData<CategoryEntityData> category, EntityData<WordEntityData> word)
        {
            if (Entities.ContainsKey(category.Id) is false)
            {
                Debug.LogWarning($"[CategoryEntity::AddWord] There is no category with name: {category.Data.Title}");
                return;
            }
        
            category.Data.Words.Add(word);
        }

        protected override async Task LoadEntity()
        {
            try
            {
                var customCategory =
                    await dbController.ReadData<Dictionary<string, EntityData<CategoryEntityData>>>(GetPath(userId));
                if (customCategory != null)
                    Entities = customCategory;

                var globalCategory =
                    await dbController
                        .ReadData<Dictionary<string, EntityData<CategoryEntityData>>>("global_categories");
                if (globalCategory != null)
                    Entities = globalCategory;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading entities: {e.Message}");
            }
        }
    }
}