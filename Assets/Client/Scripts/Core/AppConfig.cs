using Client.Scripts.Patterns.ResourceLoader.ConfigLoader;
using UnityEngine;

namespace Client.Scripts.Core
{
    internal sealed class AppConfig : ScriptableObject
    {
        internal static AppConfig Instance => _instance ?? (_instance = ConfigLoader.LoadAppConfig<AppConfig>());
        private static AppConfig _instance;

        [field: SerializeField]
        internal string WebClientId { get; private set; }
            = "488687700276-djos9qkmcv8glof35178j3fv8p4vritr.apps.googleusercontent.com";

        [field: SerializeField] internal string SceneFolder { get; private set; } = "Assets/Client/Scenes/";
        [field: SerializeField] internal bool CleanUpTests { get; private set; } = true;
    }
}