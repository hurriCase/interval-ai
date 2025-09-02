using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Editor.ProceduralImage
{
    /// <summary>
    /// This class adds a Menu Item "GameObject/UI/Procedural Image"
    /// Behaviour of this command is the same as with regular Images
    /// </summary>
    public static class ProceduralImageEditorUtility
    {
        [MenuItem("GameObject/UI/Procedural Image")]
        public static void AddProceduralImage()
        {
            var obj = new GameObject();
            obj.AddComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>();
            obj.layer = LayerMask.NameToLayer("UI");
            obj.name = "Procedural Image";

            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponentInParent<Canvas>() != null)
            {
                obj.transform.SetParent(Selection.activeGameObject.transform, false);
                Selection.activeGameObject = obj;
            }
            else
            {
                if (Object.FindObjectOfType<Canvas>() == null)
                    EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");

                var canvas = Object.FindObjectOfType<Canvas>();

                //Set Texcoord shader channels for canvas
                canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 |
                                                   AdditionalCanvasShaderChannels.TexCoord2 |
                                                   AdditionalCanvasShaderChannels.TexCoord3;

                obj.transform.SetParent(canvas.transform, false);
                Selection.activeGameObject = obj;
            }
        }

        /// <summary>
        /// Replaces an Image Component with a Procedural Image Component.
        /// </summary>
        [MenuItem("CONTEXT/Image/Replace with Procedural Image")]
        public static void ReplaceWithProceduralImage(MenuCommand command)
        {
            var image = (Image)command.context;
            var obj = image.gameObject;
            Object.DestroyImmediate(image);
            obj.AddComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>();
        }
    }
}