using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal sealed class IndexedReactiveProperty<TKey, TValue> : ReactiveProperty<TValue>
        where TKey : unmanaged, Enum
    {
        private readonly ReactiveProperty<EnumArray<TKey, TValue>> _source;
        private readonly TKey _key;

        internal IndexedReactiveProperty(ReactiveProperty<EnumArray<TKey, TValue>> source, TKey key)
        {
            _source = source;
            _key = key;

            _source.Subscribe(this, static (array, self) => self.Value = array[self._key]);
        }

        protected override void OnNextCore(TValue value)
        {
            _source.Value[_key] = value;
            _source.OnNext(_source.Value);
            base.OnNextCore(value);
        }
    }
}