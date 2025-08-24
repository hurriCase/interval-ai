using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Source.Scripts.Core.Others
{
    internal static class AspectRatioFitterExtensions
    {
        internal static void CreateWidthSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container)
        {
            var createdSpacing = Object.Instantiate(aspectRatioFitter, container);
            createdSpacing.aspectRatio = spacing;
            createdSpacing.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        }

        internal static void CreateHeightSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container)
        {
            var createdSpacing = Object.Instantiate(aspectRatioFitter, container);
            createdSpacing.aspectRatio = spacing;
            createdSpacing.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        }

        internal static void CreateSpacing(
            this AspectRatioFitter aspectRatioFitter,
            float spacing,
            RectTransform container,
            AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth)
        {
            switch (aspectMode)
            {
                case AspectRatioFitter.AspectMode.WidthControlsHeight:
                    CreateWidthSpacing(aspectRatioFitter, spacing, container);
                    break;
                case AspectRatioFitter.AspectMode.HeightControlsWidth:
                    CreateHeightSpacing(aspectRatioFitter, spacing, container);
                    break;
            }
        }
    }
}