using System;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    // I hate this, but I didn't find a better solution to use size fitter + layout group +
    // layout element on child with flexible values, it doesn't work and extend the first child to the entire
    // container size
    [ExecuteAlways]
    public sealed class ChildSizeCopier : RectTransformBehaviour
    {
        [SerializeField] private RectTransform _targetChild;
        [SerializeField] private DimensionType _dimensionToCopy;

        private void OnRectTransformDimensionsChange()
        {
            TryCopySize();
        }

        private void TryCopySize()
        {
            if (_dimensionToCopy == DimensionType.None)
                return;

            switch (_dimensionToCopy)
            {
                case DimensionType.Width:
                    var childWidth = _targetChild.rect.width;
                    if (childWidth.IsReasonable())
                        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, childWidth);
                    break;

                case DimensionType.Height:
                    var childHeight = _targetChild.rect.height;
                    if (childHeight.IsReasonable())
                        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, childHeight);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}