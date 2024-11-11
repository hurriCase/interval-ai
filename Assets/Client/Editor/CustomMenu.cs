using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

internal sealed class CustomMenu : MonoBehaviour
{
    private const string SceneFolder = "Assets/Client/Scenes/";

    [MenuItem("Project/Scenes/First")]
    public static void OpenFirstScene() => OpenScene("First");

    [MenuItem("Project/Scenes/Main")]
    public static void OpenMainScene() => OpenScene("Main");

    private static void OpenScene(string sceneName)
    {
        var scenePath = $"{SceneFolder}{sceneName}.unity";

        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    }
}