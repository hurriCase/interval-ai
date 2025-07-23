using CustomUtils.Runtime.CustomBehaviours;
using PrimeTween;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Swipe
{
    internal sealed class SwipeCardBehaviour : RectTransformBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float _returnDuration;
        [SerializeField] private float _swipeExecuteDuration;
        [SerializeField] private float _maxRotationDegrees;
        [SerializeField] private float _maxLiftHeightRatio;
        [SerializeField] private float _pickupScale;
        [SerializeField] private float _pickupDuration;

        internal Observable<SwipeDirection> OnSwipe => _onSwipe.AsObservable();

        private readonly Subject<SwipeDirection> _onSwipe = new();

        private Vector2 _startPosition;
        private Vector2 _originalPosition;
        private bool _swipeExecuted;
        private Sequence _currentSequence;
        private SwipeDirection _currentSwipeDirection;

        private void Awake()
        {
            _originalPosition = RectTransform.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_swipeExecuted)
                return;

            _startPosition = eventData.position;

            _currentSequence.Stop();

            _currentSequence = Sequence.Create().Chain(Tween.Scale(RectTransform, _pickupScale, _pickupDuration));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_swipeExecuted)
                return;

            var currentPosition = eventData.position;
            var deltaPosition = currentPosition - _startPosition;

            var maxDistance = RectTransform.rect.width / 2;
            var normalizedDrag = deltaPosition.x / maxDistance;
            var dragIntensity = Mathf.Clamp01(Mathf.Abs(normalizedDrag));
            var dragDirection = Mathf.Sign(normalizedDrag);

            var newPosition = _originalPosition + new Vector2(deltaPosition.x, 0);
            var maxLiftHeight = RectTransform.rect.height * _maxLiftHeightRatio;
            var liftAmount = Mathf.Sin(dragIntensity * Mathf.PI * 0.5f) * maxLiftHeight;
            newPosition.y = _originalPosition.y + liftAmount;
            RectTransform.anchoredPosition = newPosition;

            var easedDrag = Mathf.Sin(dragIntensity * Mathf.PI * 0.5f) * dragDirection;
            var rotation = -easedDrag * _maxRotationDegrees;
            RectTransform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_swipeExecuted)
                return;

            var deltaX = eventData.position.x - _startPosition.x;
            var distanceForSwipe = RectTransform.rect.width / 2;
            if (Mathf.Abs(deltaX) >= distanceForSwipe)
                ExecuteSwipe(deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left);
            else
                ReturnToOriginalPosition();
        }

        private void ExecuteSwipe(SwipeDirection direction)
        {
            _swipeExecuted = true;

            var targetX = direction == SwipeDirection.Right ? Screen.width : -Screen.width;
            var targetPosition = _originalPosition + new Vector2(targetX, 0);
            var targetRotation = Quaternion.Euler(0, 0,
                direction == SwipeDirection.Right ? -_maxRotationDegrees : _maxRotationDegrees);

            _currentSwipeDirection = direction;

            _currentSequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPosition(RectTransform, targetPosition, _swipeExecuteDuration))
                .Group(Tween.Rotation(RectTransform, targetRotation, _swipeExecuteDuration))
                .OnComplete(this, static behaviour =>
                {
                    behaviour._onSwipe.OnNext(behaviour._currentSwipeDirection);
                    behaviour.ResetCard();
                });
        }

        private void ReturnToOriginalPosition()
        {
            _currentSequence.Stop();

            _currentSequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPosition(RectTransform, _originalPosition, _returnDuration))
                .Group(Tween.Rotation(RectTransform, Quaternion.identity, _returnDuration))
                .Group(Tween.Scale(RectTransform, Vector3.one, _returnDuration));
        }

        private void ResetCard()
        {
            _swipeExecuted = false;

            _currentSequence.Stop();

            RectTransform.anchoredPosition = _originalPosition;
            RectTransform.rotation = Quaternion.identity;
            RectTransform.localScale = Vector3.one;
        }

        private void OnDestroy()
        {
            _onSwipe?.Dispose();
        }
    }
}