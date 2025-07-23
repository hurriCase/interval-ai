using System;
using UnityEngine;

namespace Source.Scripts.Core.Localization
{
    [Serializable]
    internal struct LocalizationData<TEnum> where TEnum : Enum
    {
        [field: SerializeField] internal string Key { get; private set; }
        [field: SerializeField] internal TEnum Type { get; private set; }
    }
}