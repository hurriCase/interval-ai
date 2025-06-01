using CustomUtils.Editor.CustomMenu.MenuItems.Helpers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor_Default_Resources.CustomMenu.Scripts.Editor
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

        [MenuItem("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", priority = 1)]
        private static void ToggleSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED()
        {
            ScriptingSymbolHandler.ToggleSymbol("IS_RECTTRANSFORM_EXTENDED_ENABLED", "ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED");
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", true)]
        private static bool ValidateToggleSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED()
        {
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED", false));
            return true;
        }
    }
}