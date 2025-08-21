using System;
using System.Linq;
using CustomUtils.Runtime.UI;
using R3.Triggers;
using Source.Scripts.Core.Others;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components
{
    internal sealed class SizeCopierComponent : MonoBehaviour
    {
        [SerializeField] private DimensionType _dimensionType;

        [SerializeField] private RectTransform _targetForObserving;
        [SerializeField] private RectTransform[] _sources;
        [SerializeField] private RectTransform _target;

        private void Awake()
        {
            _targetForObserving.OnRectTransformDimensionsChangeAsObservable()
                .SubscribeAndRegister(this, static self => self.UpdateTargetHeight());
        }

        private void UpdateTargetHeight()
        {
            if (_dimensionType == DimensionType.None)
                return;

            _target.sizeDelta = _dimensionType switch
            {
                DimensionType.Width
                    => new Vector2(_sources.Sum(source => source.rect.width), _target.sizeDelta.y),

                DimensionType.Height
                    => new Vector2(_target.sizeDelta.x, _sources.Sum(source => source.rect.height)),

                _ => throw new ArgumentOutOfRangeException()
            };

            LayoutRebuilder.ForceRebuildLayoutImmediate(_target.parent as RectTransform);
        }
    }
}