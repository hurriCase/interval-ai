using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
    [Resource("Assets/Client/Scriptables/Resources", nameof(ThemeEditorSettings))]
    internal sealed class ThemeEditorSettings : SingletonScriptableObject<ThemeEditorSettings>
    {
        [field: SerializeField] internal Color HighlightColor { get; set; } = Color.yellow;
        [field: SerializeField] internal Color BackgroundColor { get; set; } = Color.white;
        [field: SerializeField] internal Color BorderColor { get; set; } = new(0.5f, 0.5f, 0.5f, 1f);

        [field: SerializeField] internal float HeaderSpacing { get; set; } = 10f;
        [field: SerializeField] internal int HeaderFontSize { get; set; } = 14;
        [field: SerializeField] internal FontStyle HeaderFontStyle { get; set; } = FontStyle.Bold;
        [field: SerializeField] internal TextAnchor HeaderAlignment { get; set; } = TextAnchor.MiddleLeft;

        [field: SerializeField] internal float ButtonHeight { get; set; } = 30f;
        [field: SerializeField] internal int ButtonFontSize { get; set; } = 12;
        [field: SerializeField] internal FontStyle ButtonFontStyle { get; set; } = FontStyle.Normal;
        [field: SerializeField] internal Color ButtonHighlightColor { get; set; } = Color.yellow;
        [field: SerializeField] internal Color ButtonBackgroundColor { get; set; } = Color.white;

        [field: SerializeField] internal float PanelSpacing { get; set; } = 8f;

        [field: SerializeField] internal float PropertyHeight { get; set; } = 18f;
        [field: SerializeField] internal int PropertyFontSize { get; set; } = 12;
        [field: SerializeField] internal FontStyle PropertyFontStyle { get; set; } = FontStyle.Normal;

        [field: SerializeField] internal float DropdownHeight { get; set; } = 20f;
        [field: SerializeField] internal int DropdownFontSize { get; set; } = 12;
        [field: SerializeField] internal FontStyle DropdownFontStyle { get; set; } = FontStyle.Normal;

        [field: SerializeField] internal float MessageBoxSpacing { get; set; } = 5f;

        [field: SerializeField] internal float ColorFieldHeight { get; set; } = 18f;
        [field: SerializeField] internal float ColorFieldSpacing { get; set; } = 5f;

        [field: SerializeField] internal int BoxPaddingLeft { get; set; } = 10;
        [field: SerializeField] internal int BoxPaddingRight { get; set; } = 10;
        [field: SerializeField] internal int BoxPaddingTop { get; set; } = 10;
        [field: SerializeField] internal int BoxPaddingBottom { get; set; } = 10;
        [field: SerializeField] internal float BoxSpacingBefore { get; set; } = 10f;
        [field: SerializeField] internal float BoxSpacingAfter { get; set; } = 10f;
        [field: SerializeField] internal float BoxTitleSpacing { get; set; } = 5f;
        [field: SerializeField] internal float BoxContentSpacing { get; set; } = 8f;
        [field: SerializeField] internal int BoxHeaderFontSize { get; set; } = 13;
        [field: SerializeField] internal FontStyle BoxHeaderFontStyle { get; set; } = FontStyle.Bold;
        [field: SerializeField] internal TextAnchor BoxHeaderAlignment { get; set; } = TextAnchor.MiddleLeft;

        [field: SerializeField] internal int FoldoutBoxPaddingLeft { get; set; } = 25;
        [field: SerializeField] internal int FoldoutBoxPaddingRight { get; set; } = 10;
        [field: SerializeField] internal int FoldoutBoxPaddingTop { get; set; } = 10;
        [field: SerializeField] internal int FoldoutBoxPaddingBottom { get; set; } = 10;
        [field: SerializeField] internal float FoldoutBoxSpacingBefore { get; set; } = 5f;
        [field: SerializeField] internal float FoldoutBoxSpacingAfter { get; set; }
        [field: SerializeField] internal float FoldoutHeaderSpacing { get; set; } = 5f;
        [field: SerializeField] internal float FoldoutContentSpacing { get; set; } = 8f;
        [field: SerializeField] internal int FoldoutFontSize { get; set; } = 12;
        [field: SerializeField] internal FontStyle FoldoutFontStyle { get; set; } = FontStyle.Bold;

        [field: SerializeField] internal float DividerHeight { get; set; } = 1f;
        [field: SerializeField] internal float DividerSpacing { get; set; } = 6f;
        [field: SerializeField] internal Color DividerColor { get; set; } = new(0.5f, 0.5f, 0.5f, 1f);
    }
}