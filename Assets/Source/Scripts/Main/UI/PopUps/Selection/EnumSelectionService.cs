using System;
using System.Collections.Generic;
using R3;
using Source.Scripts.Main.UI.PopUps.Selection.LocalizationData;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal sealed class EnumSelectionService<TEnum> : ISelectionService<TEnum>
        where TEnum : unmanaged, Enum
    {
        private readonly EnumLocalizationDataBase _localizationData;

        public IReadOnlyList<TEnum> SelectionValues { get; }
        public ReadOnlyReactiveProperty<TEnum> TargetProperty => _targetProperty;

        private readonly ReactiveProperty<TEnum> _targetProperty;

        internal EnumSelectionService(
            ReactiveProperty<TEnum> targetProperty,
            EnumLocalizationDataBase localizationData,
            IReadOnlyList<TEnum> customValues = null)
        {
            SelectionValues = customValues ?? (TEnum[])Enum.GetValues(typeof(TEnum));
            _targetProperty = targetProperty;
            _localizationData = localizationData;
        }

        public string GetSelectionName(TEnum value) => _localizationData.GetLocalization(value);

        public void SetValue(TEnum value, bool isSelected)
        {
            if (isSelected is false)
                return;

            _targetProperty.Value = value;
        }

        public bool GetSelectionState(TEnum value)
            => EqualityComparer<TEnum>.Default.Equals(value, _targetProperty.Value);
    }
}