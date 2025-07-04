using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Eflatun.SceneReference;
using UnityEngine;

namespace Source.Scripts.Core.Scenes
{
    [Resource(ResourcePaths.ResourcePath, nameof(SceneReferences))]
    internal sealed class SceneReferences : SingletonScriptableObject<SceneReferences>
    {
        [field: SerializeField] internal SceneReference MainMenuScene { get; private set; }
    }
}