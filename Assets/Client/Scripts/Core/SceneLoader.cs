using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.Core
{
    internal static class SceneLoader
    {
        internal static async Task LoadStartUpScene() => await LoadScene("StartUp");
        internal static async Task LoadLoginScene() => await LoadScene("Login");
        internal static async Task LoadMainScene() => await LoadScene("Main");

        private static async Task LoadScene(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
                Debug.LogWarning($"[SceneLoader::LoadScene] You're trying to load an active scene '{sceneName}'");

            if (SceneManager.GetSceneByName(sceneName).isLoaded is false)
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
    }
}