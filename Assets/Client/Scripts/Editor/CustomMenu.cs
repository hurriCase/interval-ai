using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Client.Scripts.Editor
{
    internal sealed class CustomMenu : MonoBehaviour
    {
        private const string SceneFolder = "Assets/Client/Scenes/";

        [MenuItem("Project/Scenes/StartUp", priority = 0)]
        public static void OpenStartUpScene() => OpenScene("StartUp");

        [MenuItem("Project/Scenes/Login", priority = 1)]
        public static void OpenLoginScene() => OpenScene("Login");
        
        [MenuItem("Project/Scenes/Main", priority = 2)]
        public static void OpenMainScene() => OpenScene("Main");

        private static void OpenScene(string sceneName)
        {
            var scenePath = $"{SceneFolder}{sceneName}.unity";

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }
    }
}