using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    internal sealed class FlexibleWrapLayout : LayoutGroup
    {
        [SerializeField] private float _spacing;
        [SerializeField] private float _lineSpacing;
        [SerializeField] private TextAnchor _childAlignment;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalculateLayout();
        }

        public override void CalculateLayoutInputVertical()
        {
            CalculateLayout();
        }

        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis();
        }

        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis();
        }

        private void CalculateLayout()
        {
            var containerWidth = rectTransform.rect.width;

            var currentLineWidth = 0f;
            var currentLineHeight = 0f;
            var totalHeight = 0f;
            var maxWidth = 0f;

            foreach (var child in rectChildren)
            {
                var childWidth = LayoutUtility.GetPreferredWidth(child);
                var childHeight = LayoutUtility.GetPreferredHeight(child);

                if (currentLineWidth + childWidth > containerWidth && currentLineWidth > 0f)
                {
                    totalHeight += currentLineHeight + _lineSpacing;
                    maxWidth = Mathf.Max(maxWidth, currentLineWidth);
                    currentLineWidth = 0f;
                    currentLineHeight = 0f;
                }

                currentLineWidth += childWidth + (currentLineWidth > 0f ? _spacing : 0f);
                currentLineHeight = Mathf.Max(currentLineHeight, childHeight);
            }

            totalHeight += currentLineHeight;
            maxWidth = Mathf.Max(maxWidth, currentLineWidth);

            SetLayoutInputForAxis(maxWidth, maxWidth, -1, 0);
            SetLayoutInputForAxis(totalHeight, totalHeight, -1, 1);
        }

        private void SetChildrenAlongAxis()
        {
            var containerWidth = rectTransform.rect.width;

            var xPos = 0f;
            var yPos = 0f;
            var currentLineHeight = 0f;

            foreach (var child in rectChildren)
            {
                var childWidth = LayoutUtility.GetPreferredWidth(child);
                var childHeight = LayoutUtility.GetPreferredHeight(child);

                if (xPos + childWidth > containerWidth && xPos > 0f)
                {
                    yPos += currentLineHeight + _lineSpacing;
                    xPos = 0f;
                    currentLineHeight = 0f;
                }

                currentLineHeight = Mathf.Max(currentLineHeight, childHeight);

                SetChildAlongAxis(child, 0, xPos, childWidth);
                SetChildAlongAxis(child, 1, yPos, childHeight);

                xPos += childWidth + _spacing;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            CalculateLayoutInputHorizontal();
        }
#endif
    }
}