using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.AssetLoader.Config;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Core
{
    [Resource("Assets/Resource/Config", "AssetLoaderConfig", "Configs")]
    internal sealed class AssetLoaderConfig : ScriptableSingleton<AssetLoaderConfig>, IAssetLoaderConfig
    {
        [field: SerializeField] public string DontDestroyPath { get; private set; } = "DontDestroyOnLoad";
    }
}