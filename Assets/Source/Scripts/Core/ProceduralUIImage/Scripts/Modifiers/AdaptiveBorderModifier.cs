﻿using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace ProceduralUIImage.Scripts.Modifiers
{
    [ModifierID("Adaptive Border")]
    public sealed class AdaptiveBorderModifier : ProceduralImageModifier
    {
        [field: SerializeField] public float CornerRadiusRatio { get; private set; }
        [field: SerializeField] public float DesiredRadius { get; private set; }

        public override Vector4 CalculateRadius(Rect imageRect)
        {
            var minSide = Mathf.Min(imageRect.width, imageRect.height);
            var actualRadius = minSide * CornerRadiusRatio;

            var maxAllowedRadius = minSide * 0.5f;
            actualRadius = Mathf.Min(actualRadius, maxAllowedRadius);

            return new Vector4(actualRadius, actualRadius, actualRadius, actualRadius);
        }
    }
}