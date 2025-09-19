using R3;
using UnityEngine;

namespace Source.Scripts.Core.Input
{
    internal interface ISwipeInputService
    {
        Observable<Unit> OnPointerPressed { get; }
        Observable<Unit> OnPointerReleased { get; }
        Observable<Vector2> OnPointerPositionChanged { get; }
        Vector2 CurrentPointerPosition { get; }
    }
}