using CustomUtils.Runtime.UI;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Source.Scripts.Core.Others
{
    internal static class RectTransformExtensions
    {
        internal static Observable<float> OnReactTransformHeightChangeAsObservable(this RectTransform target)
        {
            return target.OnRectTransformDimensionsChangeAsObservable()
                .Select(target, static (_, target) => GetDimensionValue(target, DimensionType.Height))
                .DistinctUntilChanged();
        }

        internal static Observable<float> OnReactTransformWidthChangeAsObservable(this RectTransform target)
        {
            return target.OnRectTransformDimensionsChangeAsObservable()
                .Select(target, static (_, target) => GetDimensionValue(target, DimensionType.Width))
                .DistinctUntilChanged();
        }

        internal static Observable<float> OnReactTransformDimensionChangeAsObservable(
            this RectTransform target,
            DimensionType dimension)
        {
            return target.OnRectTransformDimensionsChangeAsObservable()
                .Select((target, dimensionToCopy: dimension),
                    static (_, tuple) => GetDimensionValue(tuple.target, tuple.dimensionToCopy))
                .DistinctUntilChanged();
        }

        private static float GetDimensionValue(RectTransform rectTransform, DimensionType dimension) =>
            dimension switch
            {
                DimensionType.Width => rectTransform.rect.width,
                DimensionType.Height => rectTransform.rect.height,
                _ => 0f
            };
    }
}