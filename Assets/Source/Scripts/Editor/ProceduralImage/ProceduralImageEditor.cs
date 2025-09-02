using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Source.Scripts.Editor.ProceduralImage
{
    [CustomEditor(typeof(UnityEngine.UI.ProceduralImage.ProceduralImage), true)]
    [CanEditMultipleObjects]
    public class ProceduralImageEditor : ImageEditor
    {
        private static List<ModifierID> _attributes;

        SerializedProperty m_borderWidth;
        SerializedProperty m_falloffDist;

        SerializedProperty m_FillMethod;
        SerializedProperty m_FillOrigin;
        SerializedProperty m_FillAmount;
        SerializedProperty m_FillClockwise;
        SerializedProperty m_Type;
        SerializedProperty m_Sprite;

        GUIContent spriteTypeContent = new GUIContent("Image Type");
        GUIContent clockwiseContent = new GUIContent("Clockwise");

        AnimBool showFilled;
        int selectedId;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_Type = serializedObject.FindProperty("m_Type");
            m_FillMethod = serializedObject.FindProperty("m_FillMethod");
            m_FillOrigin = serializedObject.FindProperty("m_FillOrigin");
            m_FillClockwise = serializedObject.FindProperty("m_FillClockwise");
            m_FillAmount = serializedObject.FindProperty("m_FillAmount");
            m_Sprite = serializedObject.FindProperty("m_Sprite");

            var typeEnum = (Image.Type)m_Type.enumValueIndex;

            showFilled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == Image.Type.Filled);
            showFilled.valueChanged.AddListener(Repaint);

            _attributes = ModifierUtility.GetAttributeList();

            m_borderWidth = serializedObject.FindProperty("borderRatio");
            m_falloffDist = serializedObject.FindProperty("falloffDistance");

            if ((target as UnityEngine.UI.ProceduralImage.ProceduralImage).GetComponent<ProceduralImageModifier>() != null)
            {
                selectedId = _attributes.IndexOf(((ModifierID[])(target as UnityEngine.UI.ProceduralImage.ProceduralImage)
                    .GetComponent<ProceduralImageModifier>().GetType()
                    .GetCustomAttributes(typeof(ModifierID), false))[0]);
            }

            selectedId = Mathf.Max(selectedId, 0);
            EditorApplication.update -= UpdateProceduralImage;
            EditorApplication.update += UpdateProceduralImage;
        }

        /// <summary>
        /// Updates the procedural image in Edit mode. This will prevent issues when working with layout components.
        /// </summary>
        public void UpdateProceduralImage()
        {
            if (target != null)
            {
                (target as UnityEngine.UI.ProceduralImage.ProceduralImage).Update();
            }
            else
            {
                EditorApplication.update -= UpdateProceduralImage;
            }
        }

        public override void OnInspectorGUI()
        {
            CheckForShaderChannelsGUI();

            serializedObject.Update();

            ProceduralImageSpriteGUI();

            EditorGUILayout.PropertyField(m_Color);

            RaycastControlsGUI();
            ProceduralImageTypeGUI();
            EditorGUILayout.Space();
            ModifierGUI();
            EditorGUILayout.PropertyField(m_borderWidth);
            EditorGUILayout.PropertyField(m_falloffDist);
            serializedObject.ApplyModifiedProperties();
        }

        protected void ProceduralImageSpriteGUI()
        {
            if (m_Sprite.hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(m_Sprite);
            }
            else
            {
                var sprite = (Sprite)EditorGUILayout.ObjectField("Sprite",
                    EmptySprite.IsEmptySprite((Sprite)m_Sprite.objectReferenceValue)
                        ? null
                        : m_Sprite.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(16));

                m_Sprite.objectReferenceValue = sprite != null ? sprite : EmptySprite.GetSprite();
            }
        }

        /// <summary>
        /// Sprites's custom properties based on the type.
        /// </summary>
        protected void ProceduralImageTypeGUI()
        {
            if (m_Type.hasMultipleDifferentValues)
            {
                var idx = Convert.ToInt32(EditorGUILayout.EnumPopup(spriteTypeContent, (ProceduralImageType)(-1)));
                if (idx != -1)
                    m_Type.enumValueIndex = idx;
            }
            else
            {
                m_Type.enumValueIndex =
                    Convert.ToInt32(EditorGUILayout.EnumPopup(spriteTypeContent,
                        (ProceduralImageType)m_Type.enumValueIndex));
            }

            ++EditorGUI.indentLevel;
            {
                var typeEnum = (Image.Type)m_Type.enumValueIndex;

                showFilled.target = !m_Type.hasMultipleDifferentValues && typeEnum == Image.Type.Filled;

                if (EditorGUILayout.BeginFadeGroup(showFilled.faded))
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(m_FillMethod);

                    if (EditorGUI.EndChangeCheck())
                        m_FillOrigin.intValue = 0;

                    switch ((Image.FillMethod)m_FillMethod.enumValueIndex)
                    {
                        case Image.FillMethod.Horizontal:
                            m_FillOrigin.intValue =
                                (int)(Image.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin",
                                    (Image.OriginHorizontal)m_FillOrigin.intValue);
                            break;
                        case Image.FillMethod.Vertical:
                            m_FillOrigin.intValue =
                                (int)(Image.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin",
                                    (Image.OriginVertical)m_FillOrigin.intValue);
                            break;
                        case Image.FillMethod.Radial90:
                            m_FillOrigin.intValue =
                                (int)(Image.Origin90)EditorGUILayout.EnumPopup("Fill Origin",
                                    (Image.Origin90)m_FillOrigin.intValue);
                            break;
                        case Image.FillMethod.Radial180:
                            m_FillOrigin.intValue =
                                (int)(Image.Origin180)EditorGUILayout.EnumPopup("Fill Origin",
                                    (Image.Origin180)m_FillOrigin.intValue);
                            break;
                        case Image.FillMethod.Radial360:
                            m_FillOrigin.intValue =
                                (int)(Image.Origin360)EditorGUILayout.EnumPopup("Fill Origin",
                                    (Image.Origin360)m_FillOrigin.intValue);
                            break;
                    }

                    EditorGUILayout.PropertyField(m_FillAmount);
                    if ((Image.FillMethod)m_FillMethod.enumValueIndex > Image.FillMethod.Vertical)
                        EditorGUILayout.PropertyField(m_FillClockwise, clockwiseContent);
                }

                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;
        }

        void CheckForShaderChannelsGUI()
        {
            var c = (target as Component).GetComponentInParent<Canvas>();
            if (c != null && (c.additionalShaderChannels
                              | AdditionalCanvasShaderChannels.TexCoord1
                              | AdditionalCanvasShaderChannels.TexCoord2
                              | AdditionalCanvasShaderChannels.TexCoord3) != c.additionalShaderChannels)
            {
                //Texcoord1 not enabled;
                EditorGUILayout.HelpBox(
                    "TexCoord1,2,3 are not enabled as an additional shader channel in parent canvas. Procedural Image will not work properly",
                    MessageType.Error);
                if (GUILayout.Button("Fix: Enable TexCoord1,2,3 in Canvas: " + c.name))
                {
                    Undo.RecordObject(c, "enable TexCoord1,2,3 as additional shader channels");
                    c.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 |
                                                  AdditionalCanvasShaderChannels.TexCoord2 |
                                                  AdditionalCanvasShaderChannels.TexCoord3;
                }
            }
        }

        protected void ModifierGUI()
        {
            var con = new GUIContent[_attributes.Count];
            for (var i = 0; i < con.Length; i++)
                con[i] = new GUIContent(_attributes[i].Name);

            var hasMultipleValues = false;
            if (targets.Length > 1)
            {
                var t = (targets[0] as UnityEngine.UI.ProceduralImage.ProceduralImage).GetComponent<ProceduralImageModifier>().GetType();
                foreach (var item in targets)
                {
                    if ((item as UnityEngine.UI.ProceduralImage.ProceduralImage).GetComponent<ProceduralImageModifier>().GetType() != t)
                    {
                        hasMultipleValues = true;
                        break;
                    }
                }
            }

            if (!hasMultipleValues)
            {
                var index = EditorGUILayout.Popup(new GUIContent("Modifier Type"), selectedId, con);
                if (selectedId != index)
                {
                    selectedId = index;
                    foreach (var item in targets)
                    {
                        (item as UnityEngine.UI.ProceduralImage.ProceduralImage).ModifierType =
                            ModifierUtility.GetTypeWithId(_attributes[selectedId].Name);

                        MoveComponentBehind((item as UnityEngine.UI.ProceduralImage.ProceduralImage),
                            (item as UnityEngine.UI.ProceduralImage.ProceduralImage).GetComponent<ProceduralImageModifier>());
                    }

                    //Exit GUI prevents Unity from trying to draw destroyed components editor;
                    EditorGUIUtility.ExitGUI();
                }
            }
            else
            {
                var index = EditorGUILayout.Popup(new GUIContent("Modifier Type"), -1, con);
                if (index != -1)
                {
                    selectedId = index;
                    foreach (var item in targets)
                    {
                        (item as UnityEngine.UI.ProceduralImage.ProceduralImage).ModifierType =
                            ModifierUtility.GetTypeWithId(_attributes[selectedId].Name);

                        MoveComponentBehind((item as UnityEngine.UI.ProceduralImage.ProceduralImage),
                            (item as UnityEngine.UI.ProceduralImage.ProceduralImage).GetComponent<ProceduralImageModifier>());
                    }

                    //Exit GUI prevents Unity from trying to draw destroyed components editor;
                    EditorGUIUtility.ExitGUI();
                }
            }
        }

        public override string GetInfoString()
        {
            var image = target as UnityEngine.UI.ProceduralImage.ProceduralImage;
            return $"Modifier: {_attributes[selectedId].Name}, Line-Weight: {image.BorderRatio}";
        }

        /// <summary>
        /// Moves a component behind a reference component.
        /// </summary>
        /// <param name="reference">Reference component.</param>
        /// <param name="componentToMove">Component to move.</param>
        private static void MoveComponentBehind(Component reference, Component componentToMove)
        {
            if (reference == null || componentToMove == null || reference.gameObject != componentToMove.gameObject)
                return;

            var comps = reference.GetComponents<Component>();
            var list = new List<Component>();
            list.AddRange(comps);
            var i = list.IndexOf(componentToMove) - list.IndexOf(reference);
            while (i != 1)
            {
                switch (i)
                {
                    case < 1:
                        ComponentUtility.MoveComponentDown(componentToMove);
                        i++;
                        break;
                    case > 1:
                        ComponentUtility.MoveComponentUp(componentToMove);
                        i--;
                        break;
                }
            }
        }

        private enum ProceduralImageType
        {
            Simple = 0,
            Filled = 3
        }
    }
}