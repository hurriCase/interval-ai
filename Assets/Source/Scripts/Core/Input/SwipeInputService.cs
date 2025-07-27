using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Core.Input
{
    internal sealed class SwipeInputService : ISwipeInputService, IDisposable
    {
        public Observable<Unit> PointerPressed => _pointerPressedSubject.AsObservable();
        public Observable<Unit> PointerReleased => _pointerReleasedSubject.AsObservable();
        public Observable<Vector2> PointerPositionChangedSubject => _pointerPositionChangedSubject.AsObservable();
        public Vector2 CurrentPointerPosition => _inputActions.UI.PointerPosition.ReadValue<Vector2>();

        private readonly Subject<Unit> _pointerPressedSubject = new();
        private readonly Subject<Unit> _pointerReleasedSubject = new();
        private readonly Subject<Vector2> _pointerPositionChangedSubject = new();

        private readonly InputSystemUI _inputActions;

        private readonly IDisposable _disposable;
        private bool _isPointerPressed;

        public SwipeInputService(InputSystemUI inputActions)
        {
            _inputActions = inputActions;

            _inputActions.UI.PointerPress.performed += OnPointerPressed;
            _inputActions.UI.PointerPress.canceled += OnPointerReleased;

            _disposable = Observable.EveryUpdate()
                .Where(this, static (_, service) => service._isPointerPressed)
                .Subscribe(this, static (_, service) => service.OnPointerPositionChanged());

            _inputActions.Enable();
        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            _isPointerPressed = true;
            _pointerPressedSubject.OnNext(Unit.Default);
        }

        private void OnPointerReleased(InputAction.CallbackContext context)
        {
            _isPointerPressed = false;
            _pointerReleasedSubject.OnNext(Unit.Default);
        }

        private void OnPointerPositionChanged()
        {
            var newPosition = _inputActions.UI.PointerPosition.ReadValue<Vector2>();
            _pointerPositionChangedSubject.OnNext(newPosition);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _inputActions?.Dispose();
            _pointerPressedSubject?.Dispose();
            _pointerReleasedSubject?.Dispose();
        }
    }
}