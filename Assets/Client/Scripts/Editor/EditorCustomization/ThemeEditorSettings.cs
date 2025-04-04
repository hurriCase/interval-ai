using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
    [Resource("Assets/Client/Scriptables/Resources", nameof(ThemeEditorSettings))]
    internal sealed class ThemeEditorSettings : SingletonScriptableObject<ThemeEditorSettings>
    {
        internal Color HighlightColor { get; set; } = Color.yellow;
        internal Color BackgroundColor { get; set; } = Color.white;
        internal Color BorderColor { get; set; } = new(0.5f, 0.5f, 0.5f, 1f);

        internal float HeaderSpacing { get; set; } = 10f;
        internal int HeaderFontSize { get; set; } = 14;
        internal FontStyle HeaderFontStyle { get; set; } = FontStyle.Bold;
        internal TextAnchor HeaderAlignment { get; set; } = TextAnchor.MiddleLeft;

        internal float ButtonHeight { get; set; } = 30f;
        internal int ButtonFontSize { get; set; } = 12;
        internal FontStyle ButtonFontStyle { get; set; } = FontStyle.Normal;
        internal Color ButtonHighlightColor { get; set; } = Color.yellow;
        internal Color ButtonBackgroundColor { get; set; } = Color.white;

        internal float PanelSpacing { get; set; } = 8f;

        internal float PropertyHeight { get; set; } = 18f;
        internal int PropertyFontSize { get; set; } = 12;
        internal FontStyle PropertyFontStyle { get; set; } = FontStyle.Normal;

        internal float DropdownHeight { get; set; } = 20f;
        internal int DropdownFontSize { get; set; } = 12;
        internal FontStyle DropdownFontStyle { get; set; } = FontStyle.Normal;

        internal float MessageBoxSpacing { get; set; } = 5f;

        internal float ColorFieldHeight { get; set; } = 18f;
        internal float ColorFieldSpacing { get; set; } = 5f;

        internal int BoxPaddingLeft { get; set; } = 10;
        internal int BoxPaddingRight { get; set; } = 10;
        internal int BoxPaddingTop { get; set; } = 10;
        internal int BoxPaddingBottom { get; set; } = 10;
        internal float BoxSpacingBefore { get; set; } = 10f;
        internal float BoxSpacingAfter { get; set; } = 10f;
        internal float BoxTitleSpacing { get; set; } = 5f;
        internal float BoxContentSpacing { get; set; } = 8f;
        internal int BoxHeaderFontSize { get; set; } = 13;
        internal FontStyle BoxHeaderFontStyle { get; set; } = FontStyle.Bold;
        internal TextAnchor BoxHeaderAlignment { get; set; } = TextAnchor.MiddleLeft;

        internal int FoldoutBoxPaddingLeft { get; set; } = 10;
        internal int FoldoutBoxPaddingRight { get; set; } = 10;
        internal int FoldoutBoxPaddingTop { get; set; } = 10;
        internal int FoldoutBoxPaddingBottom { get; set; } = 10;
        internal float FoldoutBoxSpacingBefore { get; set; } = 10f;
        internal float FoldoutBoxSpacingAfter { get; set; } = 10f;
        internal float FoldoutHeaderSpacing { get; set; } = 5f;
        internal float FoldoutContentSpacing { get; set; } = 8f;
        internal int FoldoutFontSize { get; set; } = 12;
        internal FontStyle FoldoutFontStyle { get; set; } = FontStyle.Bold;

        internal float DividerHeight { get; set; } = 1f;
        internal float DividerSpacing { get; set; } = 6f;
        internal Color DividerColor { get; set; } = new(0.5f, 0.5f, 0.5f, 1f);
    }
}