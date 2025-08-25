using System;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Swipe
{
    //TODO:<Dmitriy.Sukharev> refactor
    internal sealed class SwipeCardBehaviour : RectTransformBehaviour
    {
        [Inject] private IUISettingsRepository _uiSettingsRepository;
        [Inject] private IPracticeStateService _practiceStateService;
        [Inject] private ICurrentWordsService _currentWordsService;
        [Inject] private IWordAdvanceService _wordAdvanceService;
        [Inject] private ISwipeInputService _swipeInputService;
        [Inject] private ISwipeConfig _swipeConfig;

        private Vector2 _originalPosition;
        private Camera _uiCamera;

        private SwipeDirection _currentSwipeDirection;
        private Sequence _currentSequence;
        private Vector2 _startPosition;
        private bool _swipeExecuted;
        private bool _isDragging;
        private bool _isPointerPressed;

        private WordEntry CurrentWord => _currentWordsService.CurrentWordsByState.CurrentValue[_currentPracticeState];

        private PracticeState _currentPracticeState;

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _originalPosition = RectTransform.anchoredPosition;

            _uiCamera = GetComponentInParent<Canvas>().worldCamera;

            _swipeInputService.PointerPressed
                .Where(this, static (_, behaviour) => behaviour._swipeExecuted is false)
                .SubscribeAndRegister(this, self => self.OnPointerPressed());

            _swipeInputService.PointerReleased
                .Where(this, static (_, behaviour) => behaviour._isPointerPressed || behaviour._isDragging)
                .SubscribeAndRegister(this, self => self.OnPointerReleased());

            _swipeInputService.PointerPositionChangedSubject
                .Where(this, static (_, behaviour) => behaviour.IsValidForSwipe)
                .SubscribeAndRegister(this, (position, self) => self.OnPointerPositionChanged(position));
        }

        private bool IsValidForSwipe => _swipeExecuted is false && (_isPointerPressed || _isDragging);

        private void OnPointerPressed()
        {
            if (_uiSettingsRepository.IsSwipeEnabled.Value
                && _practiceStateService.CurrentState.CurrentValue != _currentPracticeState)
                return;

            var pointerPosition = _swipeInputService.CurrentPointerPosition;
            var canvasPosition = GetScreenToCanvasPosition(pointerPosition);

            if (RectTransform.gameObject.activeInHierarchy &&
                RectTransformUtility.RectangleContainsScreenPoint(RectTransform, pointerPosition, _uiCamera) is false)
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
            var horizontalThreshold = rect.width * _swipeConfig.HorizontalDragThresholdRatio;
            var verticalTolerance = rect.height * _swipeConfig.VerticalToleranceRatio;

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
                .Chain(Tween.Scale(RectTransform, _swipeConfig.PickupScale, _swipeConfig.PickupDuration));
        }

        private void ApplyDragVisualEffects(Vector2 deltaPosition)
        {
            var maxDistance = RectTransform.rect.width / 2;
            var normalizedDrag = deltaPosition.x / maxDistance;
            var dragIntensity = Mathf.Clamp01(Mathf.Abs(normalizedDrag));
            var dragDirection = Mathf.Sign(normalizedDrag);

            var newPosition = _originalPosition + new Vector2(deltaPosition.x, 0);
            var maxLiftHeight = RectTransform.rect.height * _swipeConfig.MaxLiftHeightRatio;
            var liftAmount = Mathf.Sin(dragIntensity * Mathf.PI * 0.5f) * maxLiftHeight;
            newPosition.y = _originalPosition.y + liftAmount;
            RectTransform.anchoredPosition = newPosition;

            var easedDrag = Mathf.Sin(dragIntensity * Mathf.PI * 0.5f) * dragDirection;
            var rotation = -easedDrag * _swipeConfig.MaxRotationDegrees;
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
                direction == SwipeDirection.Right ? -_swipeConfig.MaxRotationDegrees : _swipeConfig.MaxRotationDegrees);

            _currentSwipeDirection = direction;

            _currentSequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPosition(RectTransform, targetPosition, _swipeConfig.SwipeExecuteDuration))
                .Group(Tween.Rotation(RectTransform, targetRotation, _swipeConfig.SwipeExecuteDuration))
                .OnComplete(this, static self =>
                {
                    self.HandleSwipe();
                    self.ResetCard();
                });
        }

        private void HandleSwipe()
        {
            switch (_currentSwipeDirection)
            {
                case SwipeDirection.Left:
                    _wordAdvanceService.AdvanceWord(CurrentWord, CurrentWord.LearningState == LearningState.Default);
                    break;

                case SwipeDirection.Right:
                    _wordAdvanceService.AdvanceWord(CurrentWord, CurrentWord.LearningState != LearningState.Default);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentSwipeDirection), _currentSwipeDirection, null);
            }
        }

        private void ReturnToOriginalPosition()
        {
            _currentSequence.Stop();

            var returnDuration = _swipeConfig.ReturnDuration;

            _currentSequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPosition(RectTransform, _originalPosition, returnDuration))
                .Group(Tween.Rotation(RectTransform, Quaternion.identity, returnDuration))
                .Group(Tween.Scale(RectTransform, Vector3.one, returnDuration));
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
    }
}