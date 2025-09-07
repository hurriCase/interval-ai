using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Swipe
{
    internal sealed class SwipeCardBehaviour : RectTransformBehaviour
    {
        public Observable<SwipeDirection> SwipeObservable => _swipeObservable.AsObservable();
        private readonly Subject<SwipeDirection> _swipeObservable = new();

        private Vector2 _originalPosition;
        private Rect _screenRect;

        private SwipeDirection _currentSwipeDirection;
        private Sequence _currentSequence;
        private Vector2 _startPosition;

        private SwipeState _currentState;

        private IUISettingsRepository _uiSettingsRepository;
        private IWindowsController _windowsController;
        private ISwipeInputService _swipeInputService;
        private ISwipeConfig _swipeConfig;

        [Inject]
        public void Inject(
            IUISettingsRepository uiSettingsRepository,
            IWindowsController windowsController,
            ISwipeInputService swipeInputService,
            ISwipeConfig swipeConfig)
        {
            _uiSettingsRepository = uiSettingsRepository;
            _windowsController = windowsController;
            _swipeInputService = swipeInputService;
            _swipeConfig = swipeConfig;
        }

        internal void Init()
        {
            _originalPosition = RectTransform.anchoredPosition;
            var canvas = GetComponentInParent<Canvas>();
            _screenRect = RectTransformUtility.PixelAdjustRect(RectTransform, canvas);

            _swipeInputService.PointerPressed
                .Where(this, static self => self._uiSettingsRepository.IsSwipeEnabled.Value)
                .Where(this, static self => self._windowsController.CurrentPopUpType == PopUpType.WordPractice)
                .Where(this, static self => self._currentState is not SwipeState.SwipeExecuted)
                .SubscribeAndRegister(this, self => self.OnPointerPressed());

            _swipeInputService.PointerReleased
                .Where(this, static self => self._currentState is SwipeState.PointerPressed or SwipeState.Dragging)
                .SubscribeAndRegister(this, self => self.OnPointerReleased());

            _swipeInputService.PointerPositionChangedSubject
                .Where(this, static self => self.IsValidForSwipe)
                .SubscribeAndRegister(this, (position, self) => self.OnPointerPositionChanged(position));
        }

        private bool IsValidForSwipe => _currentState is SwipeState.PointerPressed or SwipeState.Dragging;

        private void OnPointerPressed()
        {
            var pointerPosition = _swipeInputService.CurrentPointerPosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(RectTransform, pointerPosition) is false)
                return;

            _startPosition = pointerPosition;
            _currentState = SwipeState.PointerPressed;
        }

        private void OnPointerReleased()
        {
            if (_currentState is SwipeState.Dragging)
            {
                if (TryGetSwipeDirection(out var direction))
                {
                    ExecuteSwipe(direction);
                    return;
                }

                ReturnToOriginalPosition();
            }

            _currentState = SwipeState.None;
        }

        private bool TryGetSwipeDirection(out SwipeDirection direction)
        {
            var deltaPosition = _swipeInputService.CurrentPointerPosition - _startPosition;
            var distanceForSwipe = _screenRect.width / 2;

            var isHorizontalSwipe = Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y);
            var hasMinimumDistance = Mathf.Abs(deltaPosition.x) >= distanceForSwipe;

            if (isHorizontalSwipe is false || hasMinimumDistance is false)
            {
                direction = SwipeDirection.None;
                return false;
            }

            direction = deltaPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            return true;
        }

        private void OnPointerPositionChanged(Vector2 pointerPosition)
        {
            var deltaPosition = pointerPosition - _startPosition;

            if (ShouldCancelSwipeForVerticalMovement(deltaPosition))
            {
                if (_currentState is not SwipeState.Dragging)
                    return;

                ReturnToOriginalPosition();
                _currentState = SwipeState.None;
                return;
            }

            var horizontalThreshold = _screenRect.width * _swipeConfig.HorizontalDragThresholdRatio;
            var isHorizontalThresholdExceeded = Mathf.Abs(deltaPosition.x) >= horizontalThreshold;

            if (_currentState is not SwipeState.Dragging && isHorizontalThresholdExceeded)
                StartDragging();

            if (_currentState is SwipeState.Dragging)
                ApplyDragVisualEffects(deltaPosition);
        }

        private bool ShouldCancelSwipeForVerticalMovement(Vector2 deltaPosition)
        {
            var verticalTolerance = _screenRect.height * _swipeConfig.VerticalToleranceRatio;
            var verticalMovement = Mathf.Abs(deltaPosition.y);
            var movedTooFarVertically = verticalMovement > verticalTolerance;
            var isMoreVerticalThanHorizontal = Mathf.Abs(deltaPosition.x) < verticalMovement;

            return movedTooFarVertically && isMoreVerticalThanHorizontal;
        }

        private void StartDragging()
        {
            _currentState = SwipeState.Dragging;

            _currentSequence.Stop();
            _currentSequence = Sequence.Create()
                .Chain(Tween.Scale(RectTransform, _swipeConfig.PickupScale, _swipeConfig.PickupDuration));
        }

        private void ApplyDragVisualEffects(Vector2 deltaPosition)
        {
            var maxDistance = _screenRect.width * _swipeConfig.MaxDistanceRatio;
            var normalizedDrag = deltaPosition.x / maxDistance;
            var dragIntensity = Mathf.Clamp01(Mathf.Abs(normalizedDrag));
            var dragDirection = Mathf.Sign(normalizedDrag);

            RectTransform.anchoredPosition = CalculatePosition(deltaPosition, dragIntensity);
            RectTransform.rotation = CalculateRotation(dragDirection, dragIntensity);
        }

        private Vector2 CalculatePosition(Vector2 deltaPosition, float dragIntensity)
        {
            var newPosition = _originalPosition + new Vector2(deltaPosition.x, 0);

            var heightRatio = _swipeConfig.EvaluateLiftHeightRatio(dragIntensity);
            var liftAmount = heightRatio * _screenRect.height;

            newPosition.y = _originalPosition.y + liftAmount;
            return newPosition;
        }

        private Quaternion CalculateRotation(float dragDirection, float dragIntensity)
        {
            var rotationDegrees = _swipeConfig.EvaluateRotationDegrees(dragIntensity);
            var rotation = -rotationDegrees * dragDirection;

            return Quaternion.Euler(0, 0, rotation);
        }

        private void ExecuteSwipe(SwipeDirection direction)
        {
            _currentState = SwipeState.SwipeExecuted;

            var targetPosition = GetSwipeTargetPosition(direction);
            var targetRotation = GetSwipeTargetRotation(direction);

            _currentSwipeDirection = direction;
            _currentSequence = RectTransform
                .TweenPositionAndRotation(targetPosition, targetRotation, _swipeConfig.SwipeExecuteDuration)
                .OnComplete(this, static self =>
                {
                    self._swipeObservable.OnNext(self._currentSwipeDirection);
                    self.ResetCard();
                });
        }

        private Vector2 GetSwipeTargetPosition(SwipeDirection direction)
        {
            var sign = direction is SwipeDirection.Right ? 1 : -1;
            var targetX = sign * Screen.width;
            return _originalPosition + new Vector2(targetX, 0);
        }

        private Quaternion GetSwipeTargetRotation(SwipeDirection direction)
        {
            var sign = direction is SwipeDirection.Right ? -1 : 1;
            var rotationAngle = sign * _swipeConfig.GetMaxRotationDegrees();

            return Quaternion.Euler(0, 0, rotationAngle);
        }

        private void ReturnToOriginalPosition()
        {
            _currentSequence.Stop();
            _currentSequence = RectTransform.TweenToOriginalTransform(_originalPosition, _swipeConfig.ReturnDuration);
        }

        private void ResetCard()
        {
            _currentState = SwipeState.None;
            _startPosition = Vector2.zero;

            _currentSequence.Stop();

            RectTransform.anchoredPosition = _originalPosition;
            RectTransform.rotation = Quaternion.identity;
            RectTransform.localScale = Vector3.one;
        }
    }
}