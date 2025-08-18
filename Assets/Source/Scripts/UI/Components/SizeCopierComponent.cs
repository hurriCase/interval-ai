using System;
using System.Linq;
using CustomUtils.Runtime.UI;
using R3;
using R3.Triggers;
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
                .Subscribe(this, static (_, component) => component.UpdateTargetHeight())
                .RegisterTo(destroyCancellationToken);
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