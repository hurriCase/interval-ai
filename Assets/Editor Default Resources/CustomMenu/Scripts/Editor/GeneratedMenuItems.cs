using CustomUtils.Editor.CustomMenu.MenuItems.Helpers;
using CustomUtils.Editor.CustomMenu.MenuItems.MenuItems.MethodExecution;
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

        [MenuItem("References/Default Categories Database", priority = 0)]
        private static void SelectAssetDefaultCategoriesDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/LifetimeRelated/Core/DefaultCategoriesDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Localization Keys Database", priority = 1)]
        private static void SelectAssetLocalizationKeysDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/LifetimeRelated/Main/LocalizationKeysDatabase.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Progress Graph Settings", priority = 3)]
        private static void SelectAssetProgressGraphSettings()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/LifetimeRelated/Main/ProgressGraphSettings.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/SceneReferences", priority = 4)]
        private static void SelectAssetSceneReferences()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/LifetimeRelated/Core/SceneReferences.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Mappings/Calendar Text Color Mapping", priority = 1)]
        private static void SelectAssetMapping_Activity_Calendar()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/ColorMappings/Activity/Mapping_Activity_Calendar.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Mappings/Progress Color Mapping", priority = 2)]
        private static void SelectAssetMapping_ProgressColor()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/ColorMappings/Progress/Mapping_ProgressColor.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Mappings/Buttons/Gray Black Mapping", priority = 1)]
        private static void SelectAssetMapping_IconsSecondary_IconsMain()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/ColorMappings/Buttons/Mapping_IconsSecondary_IconsMain.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("References/Mappings/Buttons/Purple Gray Mapping", priority = 2)]
        private static void SelectAssetMapping_TextSecondary_BaseMain()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/Source/Scriptables/ColorMappings/Buttons/Mapping_TextSecondary_BaseMain.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("GameObject/UI Custom/Text/Regular Text _&1", priority = 2)]
        private static void CreateP_Text(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_.prefab");
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

        [MenuItem("GameObject/UI Custom/Text/Localized Text _&2", priority = 1)]
        private static void CreateP_Text_Localized(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_Localized_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Localized] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_Localized_.prefab");
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

        [MenuItem("GameObject/UI Custom/Text/Regular & Adaptive _&3", priority = 13)]
        private static void CreateP_Text_Adaptive(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_Adaptive_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Adaptive] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_Adaptive_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Adaptive] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Text/Localized & Adaptive _&4", priority = 13)]
        private static void CreateP_Text_Localized_Adaptive(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_Localized_Adaptive_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Localized_Adaptive] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Text/P_Text_Localized_Adaptive_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Text_Localized_Adaptive] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Image/Image Theme _&0", priority = 5)]
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

        [MenuItem("GameObject/UI Custom/Image/Image Pixel Per Unit & Theme  _&9", priority = 6)]
        private static void CreateP_Image_Pixel(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Pixel_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Pixel] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Pixel_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Pixel] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Image/Image Aspect Ratio _&8", priority = 7)]
        private static void CreateP_Image_Ratio(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Ratio_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Ratio] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/Image/P_Image_Ratio_.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Image_Ratio] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Spacing _&s", priority = 9)]
        private static void CreateP_Spaccing(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/P_Spaccing.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Spaccing] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/P_Spaccing.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Spaccing] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Scroll View Theme", priority = 10)]
        private static void CreateP_ScrollView(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/P_ScrollView.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_ScrollView] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/P_ScrollView.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_ScrollView] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Separator Theme _#s", priority = 11)]
        private static void CreateP_Separator(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/P_Separator.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Separator] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/P_Separator.prefab");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (!instance)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_Separator] Failed to instantiate prefab");
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

        [MenuItem("GameObject/UI Custom/Layout/Vertical Layout Group _&h", priority = 12)]
        private static void CreateP_VerticalLayout(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/HorizontalVerticalLayout/P_VerticalLayout_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_VerticalLayout] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/HorizontalVerticalLayout/P_VerticalLayout_.prefab");
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

        [MenuItem("GameObject/UI Custom/Layout/Horizontal Layout Group _&v", priority = 13)]
        private static void CreateP_HorizontalLayout(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/UI/CustomComponents/HorizontalVerticalLayout/P_HorizontalLayout_.prefab");

            if (!prefab)
            {
                Debug.LogError("[GeneratedMenuItems::CreateP_HorizontalLayout] Prefab not found at path: Assets/Source/Prefabs/UI/CustomComponents/HorizontalVerticalLayout/P_HorizontalLayout_.prefab");
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
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is RectTransform Extended Enabled", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_RECTTRANSFORM_EXTENDED_ENABLED", false));
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
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is Auto Stretch RectTransform", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_AUTO_STRETCH_RECTTRANSFORM", false));
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
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Addressables Log All", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_ADDRESSABLES_LOG_ALL", false));
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
            Menu.SetChecked("--Project--/Scripting Symbols/Toggle Is Debug", ScriptingSymbolHandler.IsSymbolEnabled("ScriptingSymbol_IS_DEBUG", false));
            return true;
        }
    }
}