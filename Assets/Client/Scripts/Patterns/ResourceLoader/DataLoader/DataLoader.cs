using System;
using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader.DataLoader
{
    internal sealed class DataLoader : ResourceLoaderBase<ScriptableObject>
    {
        private static readonly DataLoader _instance = new();
        protected override string BasePath => "Data/";

        internal static T LoadConfig<T>(string path) where T : ScriptableObject =>
            _instance.Load(path) as T;

        internal static bool TryLoadConfig<T>(string path, out T config) where T : ScriptableObject
        {
            if (_instance.TryLoad(path, out var resource))
            {
                config = resource as T;
                return config is not null;
            }

            config = null;
            return false;
        }

        internal static T[] LoadAllConfigs<T>(string subfolder = "") where T : ScriptableObject =>
            Array.ConvertAll(_instance.LoadAll(subfolder), obj => obj as T);

        internal static T LoadUserData<T>() where T : ScriptableObject => LoadConfig<T>("UserData");
    }
}