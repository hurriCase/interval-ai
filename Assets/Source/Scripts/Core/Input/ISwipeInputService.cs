using R3;
using UnityEngine;

namespace Source.Scripts.Core.Input
{
    internal interface ISwipeInputService
    {
        Observable<Unit> PointerPressed { get; }
        Observable<Unit> PointerReleased { get; }
        Observable<Vector2> PointerPositionChangedSubject { get; }
        Vector2 CurrentPointerPosition { get; }
    }
}