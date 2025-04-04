using System.Collections.Generic;
using AssetLoader.Runtime;
using Client.Scripts.UI.Base.Colors.Base;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors
{
    [Resource("Assets/Client/Scriptables/Resources/UI", nameof(ThemeColorDatabase), "UI")]
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

        internal int GetSolidColorIndexByName(string name) => SolidColors.FindIndex(color => color.Name == name);

        internal int GetGradientColorIndexByName(string name) => GradientColors.FindIndex(color => color.Name == name);

        private List<TColor> GetColorList<TColor>() where TColor : IThemeColor
        {
            return typeof(TColor) switch
            {
                var type when type == typeof(ThemeSolidColor) => SolidColors as List<TColor>,
                var type when type == typeof(ThemeGradientColor) => GradientColors as List<TColor>,
                _ => null
            };
        }
    }
}