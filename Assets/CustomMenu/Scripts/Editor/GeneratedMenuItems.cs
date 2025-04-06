using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CustomMenu.Editor;

namespace CustomMenu.Scripts.Editor
{
    internal static class GeneratedMenuItems
    {
        [MenuItem("--Project--/Scenes/Login", priority = 1)]
        private static void OpenSceneLogin()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Client/Scenes/Login.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("--Project--/Scenes/Main", priority = 2)]
        private static void OpenSceneMain()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Client/Scenes/Main.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("--Project--/Scenes/StartUp", priority = 3)]
        private static void OpenSceneStartUp()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Client/Scenes/StartUp.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("--Project--/Reference/Asset Loader Config", priority = 2)]
        private static void SelectAssetAssetLoaderConfig()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Client/Resources/Configs/AssetLoaderConfig.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Reference/DBConfig", priority = 3)]
        private static void SelectAssetDBConfig()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Client/Resources/Configs/DBConfig.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Reference/Entity Validation Config", priority = 4)]
        private static void SelectAssetEntityValidationConfig()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Client/Resources/Configs/EntityValidationConfig.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Reference/User Data", priority = 5)]
        private static void SelectAssetUserData()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Client/Resources/Data/UserData.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Reference/Theme Color Database", priority = 6)]
        private static void SelectAssetThemeColorDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Client/Scriptables/Resources/UI/Theme/ThemeColorDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Delete All Player Prefs", priority = 20)]
        private static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("--Project--/Toggle Default Auto Load", priority = 21)]
        private static void ToggleDefaultSceneAutoLoad()
        {
            DefaultSceneLoader.ToggleAutoLoad();
        }

        [MenuItem("--Project--/Toggle Default Auto Load", true)]
        private static bool ValidateToggleDefaultSceneAutoLoad()
        {
            Menu.SetChecked("--Project--/Toggle Default Auto Load", EditorPrefs.GetBool(DefaultSceneLoader.EnableSetPlayModeSceneKey, false));
            return true;
        }
    }
}