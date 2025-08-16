using System;
using System.Threading;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal static class ReactivePropertyExtensions
    {
        internal static ReactiveProperty<int> AsIndexProperty<TEnum>(
            this ReactiveProperty<TEnum> enumProperty,
            CancellationToken cancellationToken)
            where TEnum : unmanaged, Enum
        {
            var indexProperty = new ReactiveProperty<int>(UnsafeEnumConverter<TEnum>.ToInt32(enumProperty.Value));

            enumProperty
                .Subscribe(indexProperty, static (enumValue, indexProp) =>
                    indexProp.Value = UnsafeEnumConverter<TEnum>.ToInt32(enumValue))
                .RegisterTo(cancellationToken);

            indexProperty
                .Subscribe(enumProperty, static (index, enumProp) =>
                    enumProp.Value = UnsafeEnumConverter<TEnum>.FromInt32(index))
                .RegisterTo(cancellationToken);

            return indexProperty;
        }
    }
}