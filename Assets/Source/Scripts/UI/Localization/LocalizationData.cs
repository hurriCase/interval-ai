using System;
using UnityEngine;

namespace Source.Scripts.UI.Localization
{
    [Serializable]
    internal struct LocalizationData
    {
        [field: SerializeField] internal string Key { get; private set; }
        [field: SerializeField] internal LocalizationType Type { get; private set; }
    }
}