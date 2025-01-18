using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader.DontDestroyLoader
{
    internal sealed class DontDestroyLoader : ResourceLoaderBase<GameObject>
    {
        private static readonly DontDestroyLoader _instance = new();
        protected override string BasePath => "DontDestroyOnLoad/";

        public static GameObject LoadDontDestroy(string path) =>
            _instance.Load(path);

        public static bool TryLoadDontDestroy(string path, out GameObject service) =>
            _instance.TryLoad(path, out service);

        public static GameObject[] LoadAllDontDestroy() =>
            _instance.LoadAll();
    }
}