using System;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal struct SelectionParameters
    {
        internal string SelectionName { get; private set; }
        internal int[] SupportValues { get; private set; }
        internal Type EnumType { get; private set; }

        internal ReactiveProperty<int> SelectionIndex { get; private set; }

        internal void SetParameters<TEnum>(
            string selectionName,
            ReactiveProperty<TEnum> selectedIndex,
            TEnum[] supportValues,
            EnumMode enumMode,
            CancellationToken cancellationToken)
            where TEnum : unmanaged, Enum
        {
            SelectionName = selectionName;
            SelectionIndex = new ReactiveProperty<int>(UnsafeEnumConverter<TEnum>.ToInt32(selectedIndex.Value));

            selectedIndex
                .Subscribe(this, static
                    (newIndex, self)
                    => self.SelectionIndex.Value = UnsafeEnumConverter<TEnum>.ToInt32(newIndex))
                .RegisterTo(cancellationToken);

            EnumType = typeof(TEnum);

            var startIndex = enumMode == EnumMode.SkipFirst ? 1 : 0;

            SelectionIndex = new ReactiveProperty<int>(SelectionIndex.Value);

            var values = supportValues ?? (TEnum[])Enum.GetValues(typeof(TEnum));
            SupportValues = new int[values.Length - startIndex];
            for (var i = startIndex; i < values.Length; i++)
                SupportValues[i - startIndex] = UnsafeEnumConverter<TEnum>.ToInt32(values[i]);
        }
    }
}