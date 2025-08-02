using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Core.Others
{
    internal static class AspectRatioFitterExtensions
    {
        internal static void CreateSpacing(
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