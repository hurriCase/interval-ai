using Client.Scripts.UI.Base.Colors;

namespace Client.Scripts.UI.Base.Theme
{
    internal interface IBaseThemeComponent
    {
        public void ApplyColor();
        public ColorType ColorType { get; set; }
        public ThemeSolidColor ThemeSolidColor { get; set; }
        public ThemeGradientColor ThemeGradientColor { get; set; }
        public SharedColor ThemeSharedColor { get; set; }
    }
}