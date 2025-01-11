using Client.Scripts.Core;
using Client.Scripts.DB.DBControllers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Client.Scripts.Editor
{
    internal sealed class CustomMenu : MonoBehaviour
    {
        [MenuItem("Project/Scenes/StartUp", priority = 0)]
        public static void OpenStartUpScene() => OpenScene("StartUp");

        [MenuItem("Project/Scenes/Login", priority = 1)]
        public static void OpenLoginScene() => OpenScene("Login");

        [MenuItem("Project/Scenes/Main", priority = 2)]
        public static void OpenMainScene() => OpenScene("Main");

        [MenuItem("Project/DeleteAllPlayerPrefs", priority = 3)]
        public static void DeleteAllPlayerPrefs() => PlayerPrefs.DeleteAll();

        [MenuItem("Project/Configs/AppConfig", priority = 5)]
        public static void SelectAppConfig() => Selection.activeObject = AppConfig.Instance;

        [MenuItem("Project/Configs/DBConfig", priority = 6)]
        public static void SelectDBConfig() => Selection.activeObject = DBConfig.Instance;

        [MenuItem("Project/Data/UserData", priority = 7)]
        public static void SelectUserData() => Selection.activeObject = UserData.Instance;

        private static void OpenScene(string sceneName)
        {
            var scenePath = $"{AppConfig.Instance.SceneFolder}{sceneName}.unity";

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }
    }
}