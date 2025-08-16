using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;
using Source.Scripts.Core.Localization.Base;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal readonly struct SelectionParameters<TEnum> : ISelectionParameters
        where TEnum : unmanaged, Enum
    {
        public string SettingName { get; }
        public int SelectedIndex { get; }
        public int[] SupportValues { get; }

        private readonly ReactiveProperty<TEnum> _targetProperty;

        internal SelectionParameters(
            ReactiveProperty<TEnum> targetProperty,
            string settingName,
            TEnum[] supportValues = null,
            EnumMode enumMode = EnumMode.SkipFirst)
        {
            _targetProperty = targetProperty;
            SelectedIndex = UnsafeEnumConverter<TEnum>.ToInt32(targetProperty.Value);
            SettingName = settingName;

            var startIndex = enumMode == EnumMode.SkipFirst ? 1 : 0;

            supportValues ??= (TEnum[])Enum.GetValues(typeof(TEnum));

            SupportValues = new int[supportValues.Length - startIndex];

            for (var i = startIndex; i < supportValues.Length; i++)
            {
                var supportValue = supportValues[i];
                SupportValues[i - startIndex] = UnsafeEnumConverter<TEnum>.ToInt32(supportValue);
            }
        }

        public void SetValue(int enumIndex)
            => _targetProperty.Value = UnsafeEnumConverter<TEnum>.FromInt32(enumIndex);

        public string GetLocalization(ILocalizationKeysDatabase localizationKeysDatabase, int enumIndex)
            => localizationKeysDatabase.GetLocalization<TEnum>(enumIndex);
    }
}