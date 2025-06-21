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

        [MenuItem("--Project--/Assets/Image Pixel Per Unit Database", priority = 0)]
        private static void SelectAssetImagePixelPerUnitDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Resources/ImagePixelPerUnit/ImagePixelPerUnitDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Assets/Localization Keys Database", priority = 1)]
        private static void SelectAssetLocalizationKeysDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Resources/LocalizationKeysDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("--Project--/Assets/Default Categories Database", priority = 2)]
        private static void SelectAssetDefaultCategoriesDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Resources/DefaultCategoriesDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("GameObject/UI/Localized Text", priority = 1)]
        private static void CreateP_Text_Localized(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/Text/P_Text_Localized.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/Text/P_Text_Localized.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Regular Text", priority = 2)]
        private static void CreateP_Text_Regural(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/Text/P_Text_Regural.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/Text/P_Text_Regural.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Auto Size & Localized Text", priority = 3)]
        private static void CreateP_Text_Localized_AutoSize(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/Text/P_Text_Localized_AutoSize.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/Text/P_Text_Localized_AutoSize.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Image Theme", priority = 4)]
        private static void CreateP_Image_Theme(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/Image/P_Image_Theme.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/Image/P_Image_Theme.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Image Pixel Per Unit & Theme ", priority = 5)]
        private static void CreateP_Image_Theme_PixelPerUnit(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/Image/P_Image_Theme_PixelPerUnit.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/Image/P_Image_Theme_PixelPerUnit.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Image Ratio Layout", priority = 6)]
        private static void CreateP_Image_RatioLayout(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/Image/P_Image_RatioLayout.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/Image/P_Image_RatioLayout.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Ratio Layout Group", priority = 7)]
        private static void CreateP_RatioLayout_Theme(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/P_RatioLayout_Theme.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/P_RatioLayout_Theme.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Spacing", priority = 8)]
        private static void CreateP_Spaccing_Base(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/Components/Spacings/P_Spaccing_Base.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/Components/Spacings/P_Spaccing_Base.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Scroll View Theme", priority = 9)]
        private static void CreateP_ScrollView_Theme(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/P_ScrollView_Theme.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/P_ScrollView_Theme.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI/Separator Theme", priority = 10)]
        private static void CreateP_Separator_Theme(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/Elements/CustomUIs/P_Separator_Theme.prefab");

            if (!prefab)
            {
                Debug.LogError("Prefab not found at path: Assets/Source/Prefabs/UI/Elements/CustomUIs/P_Separator_Theme.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
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

        [MenuItem("--Project--/Toggle Is RectTransform Extended Enabled", priority = 1)]
        private static void ToggleSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED()
        {
            ScriptingSymbolHandler.ToggleSymbol("IS_RECTTRANSFORM_EXTENDED_ENABLED", "ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED");
        }

        [MenuItem("--Project--/Toggle Is RectTransform Extended Enabled", true)]
        private static bool ValidateToggleSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED()
        {
            Menu.SetChecked("--Project--/Toggle Is RectTransform Extended Enabled", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED", false));
            return true;
        }

        [MenuItem("--Project--/Toggle Is Auto Stretch RectTransform", priority = 2)]
        private static void ToggleSymbol_IS_AUTO_STRETCH_RECTTRANSFORM()
        {
            ScriptingSymbolHandler.ToggleSymbol("IS_AUTO_STRETCH_RECTTRANSFORM", "ScriptingSymbol_IS_AUTO_STRETCH_RECTTRANSFORM");
        }

        [MenuItem("--Project--/Toggle Is Auto Stretch RectTransform", true)]
        private static bool ValidateToggleSymbol_IS_AUTO_STRETCH_RECTTRANSFORM()
        {
            Menu.SetChecked("--Project--/Toggle Is Auto Stretch RectTransform", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_AUTO_STRETCH_RECTTRANSFORM", false));
            return true;
        }
    }
}