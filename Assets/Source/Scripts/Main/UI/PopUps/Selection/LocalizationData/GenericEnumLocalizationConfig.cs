using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [Serializable]
    internal abstract class GenericEnumLocalizationConfig<TEnum> : EnumLocalizationDataBase
        where TEnum : unmanaged, Enum
    {
        [SerializeField] protected EnumArray<TEnum, LocalizationKey> localizations = new(EnumMode.SkipFirst);

        internal override string GetLocalization<TEnumParameter>(TEnumParameter currentEnum)
        {
            var enumValue = UnsafeEnumConverter<TEnumParameter>.ToInt32(currentEnum);
            return localizations[enumValue].GetLocalization();
        }
    }
}