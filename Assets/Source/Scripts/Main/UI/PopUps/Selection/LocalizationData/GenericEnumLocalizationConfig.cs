using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Unsafe;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [Serializable]
    internal abstract class GenericEnumLocalizationConfig<TEnum> : EnumLocalizationDataBase
        where TEnum : unmanaged, Enum
    {
        [SerializeField] private EnumArray<TEnum, LocalizationKey> _localizations;

        internal override string GetLocalization<TEnumParameter>(TEnumParameter currentEnum)
        {
            var enumValue = UnsafeEnumConverter<TEnumParameter>.ToInt32(currentEnum);
            return _localizations[enumValue].GetLocalization();
        }
    }
}