using System;
using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.Core.Others
{
    [Serializable]
    internal struct SwitchablePair<TPositive, TNegative> where TPositive : Component where TNegative : Component
    {
        [field: SerializeField] internal TPositive PositiveComponent { get; private set; }
        [field: SerializeField] internal TNegative NegativeComponent { get; private set; }

        internal void Toggle(bool isPositive, bool isHideBoth)
        {
            PositiveComponent.SetActive(isPositive && isHideBoth is false);
            NegativeComponent.SetActive(isPositive is false && isHideBoth is false);
        }
    }
}