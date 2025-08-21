using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Core.Others
{
    internal static class AspectRatioFitterExtensions
    {
        internal static void CreateWidthSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container,
            AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight)
        {
            var createdSpacing = Object.Instantiate(aspectRatioFitter, container);
            createdSpacing.aspectRatio = spacing;
            createdSpacing.aspectMode = aspectMode;
        }

        internal static void CreateHeightSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container,
            AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth)
        {
            var createdSpacing = Object.Instantiate(aspectRatioFitter, container);
            createdSpacing.aspectRatio = spacing;
            createdSpacing.aspectMode = aspectMode;
        }
    }
}