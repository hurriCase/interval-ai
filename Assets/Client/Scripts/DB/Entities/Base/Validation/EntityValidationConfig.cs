using System.Collections.Generic;
using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    [Resource("Assets/Resources/Configs", "EntityValidationConfig", "Configs")]
    internal sealed class EntityValidationConfig : SingletonScriptableObject<EntityValidationConfig>
    {
        internal Dictionary<string, List<ValidationRule>> EntityRules { get; set; } = new();
    }
}