using System;
using System.Collections.Generic;
using AssetLoader.Runtime;
using Client.Scripts.UI.Base.Theme;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors
{
    [Resource("Assets/Client/Scriptables/Resources/UI/Theme", nameof(ThemeColorDatabase), "UI/Theme")]
    internal sealed class ThemeColorDatabase : SingletonScriptableObject<ThemeColorDatabase>
    {
        [field: SerializeField] internal List<ThemeSolidColor> SolidColors { get; private set; }
        [field: SerializeField] internal List<ThemeGradientColor> GradientColors { get; private set; }
        [field: SerializeField] internal List<SharedColor> SharedColor { get; private set; }

        internal string[] GetColorNames<TColor>() where TColor : IThemeColor
        {
            var colorList = GetColorList<TColor>();
            if (colorList == null || colorList.Count == 0)
                return new[] { "No colors found" };

            var names = new string[colorList.Count];
            for (var i = 0; i < colorList.Count; i++)
                names[i] = colorList[i].Name;

            return names;
        }

        internal int GetColorIndexByName(string name, ColorType colorType) =>
            colorType switch
            {
                ColorType.Shared => SharedColor.FindIndex(color => color.Name == name),
                ColorType.SolidColor => SolidColors.FindIndex(color => color.Name == name),
                ColorType.Gradient => GradientColors.FindIndex(color => color.Name == name),
                _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
            };

        private List<TColor> GetColorList<TColor>() where TColor : IThemeColor
        {
            return typeof(TColor) switch
            {
                var type when type == typeof(ThemeSolidColor) => SolidColors as List<TColor>,
                var type when type == typeof(ThemeGradientColor) => GradientColors as List<TColor>,
                var type when type == typeof(SharedColor) => SharedColor as List<TColor>,
                _ => null
            };
        }
    }
}