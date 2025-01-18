using Client.Scripts.Patterns.CustomClasses.Singletons;
using Client.Scripts.Patterns.ResourceLoader.Runtime;
using UnityEngine;

namespace Client.Scripts.Patterns
{
    [Resource("Assets/Resources", "AppConfig")]
    internal sealed class AppConfig : SingletonScriptableObject<AppConfig>
    {
        [field: SerializeField] internal Platform Platform { get; private set; }

        [field: SerializeField]
        internal string WebClientId { get; private set; }
            = "488687700276-djos9qkmcv8glof35178j3fv8p4vritr.apps.googleusercontent.com";

        [field: SerializeField] internal string SceneFolder { get; private set; } = "Assets/Client/Scenes/";
        [field: SerializeField] internal string DontDestroyResourceFolderPath { get; private set; } = "DontDestroyOnLoad";
        [field: SerializeField] internal bool CleanUpTests { get; private set; } = true;
    }

    internal enum Platform
    {
        UnityEditor,
        Android,
        Ios
    }
}