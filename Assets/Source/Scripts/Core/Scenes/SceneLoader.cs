using System;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Helpers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Source.Scripts.Core.Scenes
{
    internal static class SceneLoader
    {
        private static SceneInstance _sceneInstance;

        internal static async UniTask LoadSceneAsync(string sceneAddress, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            try
            {
#if ADDRESSABLES_LOG_ALL
                using var stopWatchScope = AddressablesLogger.LogWithTimePast("[SceneLoader::LoadSceneAsync]");
#endif

                AddressablesLogger.Log($"[SceneLoader::LoadSceneAsync] Start loading scene: {sceneAddress}");

                var currentScene = await Addressables.LoadSceneAsync(sceneAddress, loadMode);

                AddressablesLogger.Log($"[SceneLoader::LoadSceneAsync] Scene loaded successfully: {sceneAddress}");

                TryUnloadScene(_sceneInstance);

                _sceneInstance = currentScene;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader::LoadSceneAsync] Failed to load scene {sceneAddress}: {ex.Message}");
                throw;
            }
        }

        private static void TryUnloadScene(SceneInstance sceneInstance)
        {
            if (sceneInstance.Scene.IsValid() is false)
                return;

            AddressablesLogger.Log($"[SceneLoader::TryUnloadScene] Start unloading scene: {sceneInstance.Scene.name}");

            Addressables.UnloadSceneAsync(sceneInstance);

            AddressablesLogger.Log($"[SceneLoader::TryUnloadScene] Scene unloaded: {sceneInstance.Scene.name}");
        }
    }
}