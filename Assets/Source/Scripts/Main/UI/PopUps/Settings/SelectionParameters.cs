using System;
using R3;
using Source.Scripts.Core.Localization.Base;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class SelectionParameters
    {
        public string SettingName { get; set; }
        public int SelectedIndex { get; }
        public int[] SupportValues { get; }

        private readonly ReactiveProperty<int> _targetProperty;
        private readonly Func<ILocalizationKeysDatabase, int, string> _getLocalizationFunc;

        public SelectionParameters(
            int selectedIndex,
            int[] supportValues,
            ReactiveProperty<int> targetProperty,
            Func<ILocalizationKeysDatabase, int, string> getLocalizationFunc)
        {
            SelectedIndex = selectedIndex;
            SupportValues = supportValues;

            _targetProperty = targetProperty;
            _getLocalizationFunc = getLocalizationFunc;
        }

        public void SetValue(int index) => _targetProperty.Value = index;

        public string GetLocalization(ILocalizationKeysDatabase database, int index)
            => _getLocalizationFunc(database, index);
    }
}