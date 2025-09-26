using CustomUtils.Editor.Scripts.CustomMenu.MenuItems.Helpers;
using CustomUtils.Editor.Scripts.CustomMenu.MenuItems.MenuItems.MethodExecution;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor_Default_Resources.CustomMenu.Scripts.Editor
{
    internal static class GeneratedMenuItems
    {
        [MenuItem("Scenes/Startup", priority = 1)]
        private static void OpenSceneStartUp()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/StartUp.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("Scenes/Login", priority = 2)]
        private static void OpenSceneLogin()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/Login.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("Scenes/Main", priority = 3)]
        private static void OpenSceneMain()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/Main.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("Scenes/Onboarding", priority = 4)]
        private static void OpenSceneOnboarding()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/Onboarding.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("Scenes/Splash", priority = 5)]
        private static void OpenSceneSplash()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = "Assets/Source/Scenes/Splash.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("References/Default Categories Database", priority = 0)]
        private static void SelectAssetDefaultCategoriesDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/Databases/DefaultCategoriesDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Localization Keys Database", priority = 1)]
        private static void SelectAssetLocalizationKeysDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/Databases/LocalizationKeysDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Progress Graph Settings", priority = 3)]
        private static void SelectAssetProgressGraphSettings()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/Main/ProgressGraphSettings.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/SceneReferences", priority = 4)]
        private static void SelectAssetSceneReferences()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/References/SceneReferences.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Mappings/Calendar Text Color Mapping", priority = 1)]
        private static void SelectAssetMapping_Activity_Calendar()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/ColorMappings/Activity/Mapping_Activity_Calendar.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Mappings/Progress Color Mapping", priority = 2)]
        private static void SelectAssetLearningState()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/ColorMappings/Selectables/SolidColor/LearningState.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("GameObject/UI Custom/Image/Usual _&1", priority = 1)]
        private static void CreateP_Image(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Image/Theme _&2", priority = 2)]
        private static void CreateP_Image_Theme(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Theme_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Theme] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Theme_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Theme] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Image/Procedural Usual _&3", priority = 3)]
        private static void CreateP_Image_1(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/Procedural/P_Image_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_1] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/Procedural/P_Image_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_1] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Image/Procedural Theme _&4", priority = 4)]
        private static void CreateP_Image_Theme_1(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/Procedural/P_Image_Theme.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Theme_1] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/Procedural/P_Image_Theme.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Theme_1] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Image/Separator _&5", priority = 4)]
        private static void CreateP_Image_Separator(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Separator.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Separator] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Separator.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Separator] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Text/Theme Usual _#1", priority = 100)]
        private static void CreateP_Text(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Text/Theme/P_Text_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Text/Theme/P_Text_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Text/Theme Localized _#2", priority = 101)]
        private static void CreateP_Text_Localized(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Text/Theme/P_Text_Localized_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Localized] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Text/Theme/P_Text_Localized_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Localized] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Layout Group/Horizontal Layout", priority = 200)]
        private static void CreateP_HorizontalLayout(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/LayoutGroup/P_HorizontalLayout_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_HorizontalLayout] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/LayoutGroup/P_HorizontalLayout_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_HorizontalLayout] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Layout Group/Vertical Layout", priority = 201)]
        private static void CreateP_VerticalLayout(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/LayoutGroup/P_VerticalLayout_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_VerticalLayout] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/LayoutGroup/P_VerticalLayout_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_VerticalLayout] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Layout Group/Grid lLayout", priority = 202)]
        private static void CreateP_GridLayout(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/LayoutGroup/P_GridLayout_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_GridLayout] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/LayoutGroup/P_GridLayout_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_GridLayout] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Input Field", priority = 300)]
        private static void CreateP_InputField(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/P_InputField.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_InputField] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/P_InputField.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_InputField] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            Selection.activeObject = instance;
        }

        [MenuItem("GameObject/UI Custom/Slider", priority = 301)]
        private static void CreateP_Slider(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/P_Slider.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Slider] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/P_Slider.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Slider] Failed to instantiate prefab");
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                var selectedInPrefab = Selection.activeGameObject;
                if (selectedInPrefab && prefabStage.IsPartOfPrefabContents(selectedInPrefab))
                    instance.transform.SetParent(selectedInPrefab.transform);
                else
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform);

                instance.transform.localPosition = Vector3.zero;
            }
            else
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

        [MenuItem("--Project--/Delete All Stored Data", priority = 2)]
        private static void DeleteAllStoredData()
        {
            StorageHelper.TryDeleteAllAsync().Forget();
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", priority = 1)]
        private static void ToggleSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED()
        {
            ScriptingSymbolHandler.ToggleSymbol("IS_RECTTRANSFORM_EXTENDED_ENABLED", "ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED");
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", true)]
        private static bool ValidateToggleSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED()
        {
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED"));
            return true;
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is Auto Stretch RectTransform", priority = 2)]
        private static void ToggleSymbol_IS_AUTO_STRETCH_RECTTRANSFORM()
        {
            ScriptingSymbolHandler.ToggleSymbol("IS_AUTO_STRETCH_RECTTRANSFORM", "ScriptingSymbol_IS_AUTO_STRETCH_RECTTRANSFORM");
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is Auto Stretch RectTransform", true)]
        private static bool ValidateToggleSymbol_IS_AUTO_STRETCH_RECTTRANSFORM()
        {
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is Auto Stretch RectTransform", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_AUTO_STRETCH_RECTTRANSFORM"));
            return true;
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Addressables Log All", priority = 3)]
        private static void ToggleSymbol_ADDRESSABLES_LOG_ALL()
        {
            ScriptingSymbolHandler.ToggleSymbol("ADDRESSABLES_LOG_ALL", "ScriptingSymbol_ADDRESSABLES_LOG_ALL");
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Addressables Log All", true)]
        private static bool ValidateToggleSymbol_ADDRESSABLES_LOG_ALL()
        {
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Addressables Log All", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_ADDRESSABLES_LOG_ALL"));
            return true;
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is Debug", priority = 4)]
        private static void ToggleSymbol_IS_DEBUG()
        {
            ScriptingSymbolHandler.ToggleSymbol("IS_DEBUG", "ScriptingSymbol_IS_DEBUG");
        }

        [MenuItem("--Project--/Scripting Symbols/Toggle Is Debug", true)]
        private static bool ValidateToggleSymbol_IS_DEBUG()
        {
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is Debug", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_DEBUG"));
            return true;
        }
    }
}