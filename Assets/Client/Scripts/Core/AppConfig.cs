using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.Core
{
    [Resource("Assets/Resources", "AppConfig")]
    public sealed class AppConfig : SingletonScriptableObject<AppConfig>
    {
        [field: SerializeField] internal Platform Platform { get; private set; }

        [field: SerializeField]
        internal string WebClientId { get; private set; }
            = "488687700276-djos9qkmcv8glof35178j3fv8p4vritr.apps.googleusercontent.com";

        [field: SerializeField] internal string SceneFolder { get; private set; } = "Assets/Client/Scenes/";
        [field: SerializeField] internal bool CleanUpTests { get; private set; } = true;
    }

    public enum Platform
    {
        UnityEditor,
        Android,
        Ios
    }
}