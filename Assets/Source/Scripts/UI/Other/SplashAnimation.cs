using PrimeTween;
using UnityEngine;

namespace Source.Scripts.UI.Other
{
    internal sealed class SplashAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _glowCircleImageLeft;
        [SerializeField] private RectTransform _glowCircleImageRight;
        [SerializeField] private float _offset;
        [SerializeField] private float _lightSpeed;

        private void Awake()
        {
            var rect = _container.rect;
            var halfWidth = (rect.width + _offset) / 2f;
            var halfHeight = (rect.height + _offset) / 2f;

            AnimateCircle(_glowCircleImageLeft, halfWidth, halfHeight, -1);
            AnimateCircle(_glowCircleImageRight, halfWidth, halfHeight, 1);
        }

        private void AnimateCircle(RectTransform circle, float halfWidth, float halfHeight, int sign)
        {
            var corners = new[]
            {
                new Vector2(sign * halfWidth, sign * halfHeight),
                new Vector2(-sign * halfWidth, sign * halfHeight),
                new Vector2(-sign * halfWidth, -sign * halfHeight),
                new Vector2(sign * halfWidth, -sign * halfHeight)
            };

            circle.anchoredPosition = corners[0];

            var sequence = Sequence.Create(cycles: -1, sequenceEase: Ease.Linear);

            for (var i = 0; i < corners.Length; i++)
            {
                var nextIndex = (i + 1) % corners.Length;
                var nextCorner = corners[nextIndex];
                var distance = Vector2.Distance(corners[i], nextCorner);
                sequence.Chain(Tween.UIAnchoredPosition(circle, nextCorner, distance / _lightSpeed, Ease.Linear));
            }
        }
    }
}