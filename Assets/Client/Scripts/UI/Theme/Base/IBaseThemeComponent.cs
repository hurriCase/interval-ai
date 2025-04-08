using Client.Scripts.UI.Theme.ThemeColors;

namespace Client.Scripts.UI.Theme.Base
{
    internal interface IBaseThemeComponent
    {
        public void OnApplyColor();
        public ColorType ColorType { get; set; }
        public ThemeSolidColor ThemeSolidColor { get; set; }
        public ThemeGradientColor ThemeGradientColor { get; set; }
        public ThemeSharedColor ThemeSharedColor { get; set; }
    }
}