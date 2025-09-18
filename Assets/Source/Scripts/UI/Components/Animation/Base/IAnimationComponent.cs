using System;
using PrimeTween;

namespace Source.Scripts.UI.Components.Animation.Base
{
    internal interface IAnimationComponent<in TState>
        where TState : unmanaged, Enum
    {
        Tween PlayAnimation(TState state, bool isInstant);
        void CancelAnimation();
    }
}