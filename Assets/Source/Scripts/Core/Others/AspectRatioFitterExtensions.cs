using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Source.Scripts.Core.Others
{
    internal static class AspectRatioFitterExtensions
    {
        internal static AspectRatioFitter CreateWidthSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container) =>
            CreateSpacing(aspectRatioFitter, spacing, container, AspectRatioFitter.AspectMode.WidthControlsHeight);

        internal static AspectRatioFitter CreateHeightSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container) =>
            CreateSpacing(aspectRatioFitter, spacing, container);

        internal static AspectRatioFitter CreateSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container,
            AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth)
        {
            var createdSpacing = Object.Instantiate(aspectRatioFitter, container);
            createdSpacing.aspectRatio = spacing;
            createdSpacing.aspectMode = aspectMode;
            return createdSpacing;
        }
    }
}