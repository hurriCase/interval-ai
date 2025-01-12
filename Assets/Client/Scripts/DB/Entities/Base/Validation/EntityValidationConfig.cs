using System.Collections.Generic;
using Client.Scripts.Patterns.ResourceLoader.ConfigLoader;
using UnityEngine;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    [CreateAssetMenu(fileName = "EntityValidationConfig", menuName = "Configs/EntityValidationConfig")]
    internal sealed class EntityValidationConfig : ScriptableObject
    {
        internal static EntityValidationConfig Instance
            => _instance ?? (_instance = ConfigLoader.LoadEntityValidationConfig<EntityValidationConfig>());

        private static EntityValidationConfig _instance;

        internal Dictionary<string, List<ValidationRule>> EntityRules { get; set; } = new();
    }
}