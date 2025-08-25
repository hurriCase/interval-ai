using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.Base;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal sealed class EnumSelectionService<TEnum> : SelectionService<TEnum>
        where TEnum : unmanaged, Enum
    {
        public ReadOnlyReactiveProperty<TEnum> TargetProperty => _targetProperty;

        private readonly ReactiveProperty<TEnum> _targetProperty;
        private readonly ILocalizationKeysDatabase _localizationKeysDatabase;
        private readonly string _selectionTitleKey;

        internal EnumSelectionService(
            ILocalizationKeysDatabase localizationKeysDatabase,
            ReactiveProperty<TEnum> targetProperty,
            string selectionTitleKey,
            TEnum[] customValues = null,
            EnumMode enumMode = EnumMode.SkipFirst)
            : base(customValues ?? enumMode.GetEnumValues<TEnum>(), true)
        {
            _localizationKeysDatabase = localizationKeysDatabase;
            _targetProperty = targetProperty;
            _selectionTitleKey = selectionTitleKey;
        }

        public override string GetSelectionName(TEnum value)
            => _localizationKeysDatabase.GetLocalizationByValue(value);

        public override string GetSelectionTitle() => _selectionTitleKey.GetLocalization();

        public override void SetValue(TEnum value, bool isSelected)
        {
            if (isSelected is false)
                return;

            _targetProperty.Value = value;
        }

        public override bool GetSelectionState(TEnum value)
            => EqualityComparer<TEnum>.Default.Equals(value, _targetProperty.Value);
    }
}