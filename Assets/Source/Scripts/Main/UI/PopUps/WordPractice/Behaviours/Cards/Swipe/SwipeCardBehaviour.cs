using CustomUtils.Runtime.CustomBehaviours;
using PrimeTween;
using R3;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Localization;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Swipe;
using UnityEngine;
using VContainer;

namespace UI.Behaviours.Swipe
{
    //TODO:<Dmitriy.Sukharev> refactor
    internal sealed class SwipeCardBehaviour : RectTransformBehaviour
    {
        [SerializeField] private PracticeState _targetPracticeState;

        [SerializeField] private float _returnDuration;
        [SerializeField] private float _swipeExecuteDuration;
        [SerializeField] private float _maxRotationDegrees;
        [SerializeField] private float _maxLiftHeightRatio;
        [SerializeField] private float _pickupScale;
        [SerializeField] private float _pickupDuration;
        [SerializeField] private float _horizontalDragThresholdRatio;
        [SerializeField] private float _verticalToleranceRatio;

        internal Observable<SwipeDirection> OnSwipe => _onSwipe.AsObservable();

        private readonly Subject<SwipeDirection> _onSwipe = new();

        private Vector2 _originalPosition;
        private Camera _uiCamera;

        private SwipeDirection _currentSwipeDirection;
        private Sequence _currentSequence;
        private Vector2 _startPosition;
        private bool _swipeExecuted;
        private bool _isDragging;
        private bool _isPointerPressed;

        [Inject] private ISwipeInputService _swipeInputService;

        private PracticeState _currentPracticeState;

        internal void Init()
        {
            _originalPosition = RectTransform.anchoredPosition;

            _uiCamera = GetComponentInParent<Canvas>().worldCamera;

            WordPracticePopUp.CurrentState
                .Subscribe(this,
                    static (state, behaviour) => behaviour._currentPracticeState = state)
                .RegisterTo(destroyCancellationToken);

            _swipeInputService.PointerPressed
                .Where(this, static (_, behaviour) =>
                    behaviour._currentPracticeState == behaviour._targetPracticeState &&
                    behaviour._swipeExecuted is false)
                .Subscribe(this, static (_, behaviour) => behaviour.OnPointerPressed())
                .RegisterTo(destroyCancellationToken);

            _swipeInputService.PointerReleased
                .Where(this, static (_, behaviour) => behaviour._isPointerPressed || behaviour._isDragging)
                .Subscribe(this, static (_, behaviour) => behaviour.OnPointerReleased())
                .RegisterTo(destroyCancellationToken);

            _swipeInputService.PointerPositionChangedSubject
                .Where(this, static (_, behaviour) =>
                    behaviour._currentPracticeState == behaviour._targetPracticeState &&
                    behaviour._swipeExecuted is false &&
                    (behaviour._isPointerPressed || behaviour._isDragging))
                .Subscribe(this, static (position, behaviour) => behaviour.OnPointerPositionChanged(position))
                .RegisterTo(destroyCancellationToken);
        }

        private void OnPointerPressed()
        {
            var pointerPosition = _swipeInputService.CurrentPointerPosition;
            var canvasPosition = GetScreenToCanvasPosition(pointerPosition);

            if (RectTransformUtility.RectangleContainsScreenPoint(RectTransform, pointerPosition, _uiCamera) is false)
                return;

            _startPosition = canvasPosition;
            _isPointerPressed = true;
        }

        private void OnPointerReleased()
        {
            if (_isDragging)
            {
                var pointerPosition = _swipeInputService.CurrentPointerPosition;
                var canvasPosition = GetScreenToCanvasPosition(pointerPosition);

                var deltaX = canvasPosition.x - _startPosition.x;
                var deltaY = canvasPosition.y - _startPosition.y;
                var distanceForSwipe = RectTransform.rect.width / 2;

                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) && Mathf.Abs(deltaX) >= distanceForSwipe)
                {
                    ExecuteSwipe(deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left);
                    return;
                }

                ReturnToOriginalPosition();
            }

            ResetPointerStates();
        }

        private void OnPointerPositionChanged(Vector2 pointerPosition)
        {
            var canvasPosition = GetScreenToCanvasPosition(pointerPosition);
            var deltaPosition = canvasPosition - _startPosition;

            var rect = RectTransform.rect;
            var horizontalThreshold = rect.width * _horizontalDragThresholdRatio;
            var verticalTolerance = rect.height * _verticalToleranceRatio;

            if (Mathf.Abs(deltaPosition.y) > verticalTolerance &&
                Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y))
            {
                if (_isDragging is false)
                    return;

                ReturnToOriginalPosition();
                ResetPointerStates();
                return;
            }

            if (_isDragging is false && Mathf.Abs(deltaPosition.x) >= horizontalThreshold)
                StartDragging();

            if (_isDragging)
                ApplyDragVisualEffects(deltaPosition);
        }

        private void StartDragging()
        {
            _isDragging = true;

            _currentSequence.Stop();
            _currentSequence = Sequence.Create()
                .Chain(Tween.Scale(RectTransform, _pickupScale, _pickupDuration));
        }

        private void ApplyDragVisualEffects(Vector2 deltaPosition)
        {
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

        private Vector2 GetScreenToCanvasPosition(Vector3 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponentInParent<Canvas>().transform as RectTransform,
                screenPosition,
                _uiCamera,
                out var localPoint);

            return localPoint;
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

        private void ResetPointerStates()
        {
            _isPointerPressed = false;
            _isDragging = false;
        }

        private void ResetCard()
        {
            _swipeExecuted = false;
            ResetPointerStates();
            _startPosition = Vector2.zero;

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