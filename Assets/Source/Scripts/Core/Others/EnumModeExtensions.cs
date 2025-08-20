using System;
using CustomUtils.Runtime.CustomTypes.Collections;

namespace Source.Scripts.Core.Others
{
    internal static class EnumModeExtensions
    {
        internal static TEnum[] GetEnumValues<TEnum>(this EnumMode enumMode)
            where TEnum : Enum
        {
            var enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            var startIndex = enumMode == EnumMode.SkipFirst ? 1 : 0;
            return enumValues[startIndex..];
        }
    }
}