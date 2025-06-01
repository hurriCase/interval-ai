using CustomUtils.Editor.CustomMenu.MenuItems.Helpers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor_Default_Resources.CustomMenu.Scripts.Editor
{
    internal static class GeneratedMenuItems
    {
        [MenuItem("--Project--/Scenes/Startup", priority = 1)]
        private static void OpenSceneStartUp()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/StartUp.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("--Project--/Scenes/Login", priority = 2)]
        private static void OpenSceneLogin()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/Login.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("--Project--/Scenes/Main", priority = 3)]
        private static void OpenSceneMain()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/Main.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("--Project--/Toggle Default Scene Auto Load", priority = 1)]
        private static void ToggleDefaultSceneAutoLoad()
        {
            DefaultSceneLoader.ToggleAutoLoad();
        }

        [MenuItem("--Project--/Toggle Default Scene Auto Load", true)]
        private static bool ValidateToggleDefaultSceneAutoLoad()
        {
            Menu.SetChecked("--Project--/Toggle Default Scene Auto Load", DefaultSceneLoader.IsDefaultSceneSet());
            return true;
        }
    }
}