using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Core.Input
{
    internal sealed class SwipeInputService : ISwipeInputService, IDisposable
    {
        public Observable<Unit> OnPointerPressed => _pointerPressed.AsObservable();
        public Observable<Unit> OnPointerReleased => _pointerReleased.AsObservable();
        public Observable<Vector2> OnPointerPositionChanged => _pointerPositionChanged.AsObservable();
        public Vector2 CurrentPointerPosition => _inputActions.UI.PointerPosition.ReadValue<Vector2>();

        private readonly Subject<Unit> _pointerPressed = new();
        private readonly Subject<Unit> _pointerReleased = new();
        private readonly Subject<Vector2> _pointerPositionChanged = new();

        private readonly InputSystemUI _inputActions;

        private readonly IDisposable _disposable;
        private bool _isPointerPressed;

        public SwipeInputService(InputSystemUI inputActions)
        {
            _inputActions = inputActions;

            _inputActions.UI.PointerPress.performed += HandlePointerPressed;
            _inputActions.UI.PointerPress.canceled += HandlePointerReleased;

            _disposable = Observable.EveryUpdate()
                .Where(this, static (_, service) => service._isPointerPressed)
                .Subscribe(this, static (_, service) => service.HandlePointerPositionChanged());

            _inputActions.Enable();
        }

        private void HandlePointerPressed(InputAction.CallbackContext context)
        {
            _isPointerPressed = true;
            _pointerPressed.OnNext(Unit.Default);
        }

        private void HandlePointerReleased(InputAction.CallbackContext context)
        {
            _isPointerPressed = false;
            _pointerReleased.OnNext(Unit.Default);
        }

        private void HandlePointerPositionChanged()
        {
            var newPosition = _inputActions.UI.PointerPosition.ReadValue<Vector2>();
            _pointerPositionChanged.OnNext(newPosition);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _inputActions?.Dispose();
            _pointerPressed?.Dispose();
            _pointerReleased?.Dispose();
        }
    }
}