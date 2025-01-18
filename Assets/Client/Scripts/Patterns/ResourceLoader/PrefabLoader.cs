using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader
{
    internal sealed class PrefabLoader : ResourceLoaderBase<GameObject>
    {
        private static readonly PrefabLoader _instance = new();
        protected override string BasePath => "Prefabs/";

        public static GameObject LoadPrefab(string path) =>
            _instance.Load(path);

        public static bool TryLoadPrefab(string path, out GameObject prefab) =>
            _instance.TryLoad(path, out prefab);

        public static GameObject[] LoadAllPrefabs(string subfolder = "") =>
            _instance.LoadAll(subfolder);
    }
}