using System;
using PrimeTween;
using UnityEngine;

namespace Source.Scripts.UI.Components.Animation.Base
{
    [Serializable]
    internal struct AnimationData<TValue>
    {
        [field: SerializeField] internal TValue Value { get; private set; }
        [field: SerializeField] internal TweenSettings TweenSettings { get; private set; }
    }
}