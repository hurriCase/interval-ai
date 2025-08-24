using System.Collections.Generic;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.UI;
using Source.Scripts.Core.Others;
using UnityEngine;
using UnityEngine.UI;
using ZLinq;

namespace Source.Scripts.UI.Components
{
    internal sealed class SizeCopierComponent : RectTransformBehaviour
    {
        [SerializeField] private DimensionType _dimensionToCopy;
        [SerializeField] private List<RectTransform> _observedTargets;
        [SerializeField] private List<RectTransform> _sourcesToSum;

        private void Awake()
        {
            if (_dimensionToCopy == DimensionType.None)
                return;

            foreach (var target in _observedTargets)
            {
                target.OnReactTransformDimensionChangeAsObservable(_dimensionToCopy)
                    .SubscribeAndRegister(this, static self => self.UpdateTargetHeight());
            }
        }

        internal void AddObservedTarget(RectTransform observedTarget)
        {
            _observedTargets.Add(observedTarget);

            observedTarget.OnReactTransformDimensionChangeAsObservable(_dimensionToCopy)
                .SubscribeAndRegister(this, static self => self.UpdateTargetHeight());

            UpdateTargetHeight();
        }

        internal void AddSourceToSum(RectTransform sourceToSum)
        {
            _sourcesToSum.Add(sourceToSum);

            UpdateTargetHeight();
        }

        private void UpdateTargetHeight()
        {
            RectTransform.sizeDelta = _dimensionToCopy switch
            {
                DimensionType.Width => new Vector2(_sourcesToSum.AsValueEnumerable()
                    .Sum(source => source.rect.width * source.localScale.x), RectTransform.sizeDelta.y),

                DimensionType.Height => new Vector2(RectTransform.sizeDelta.x, _sourcesToSum.AsValueEnumerable()
                    .Sum(source => source.rect.height * source.localScale.y)),

                _ => RectTransform.sizeDelta
            };

            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform.parent as RectTransform);
        }
    }
}