using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using PrimeTween;
using R3;
using Source.Scripts.Core.Others;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class PopUpBase : WindowBase
    {
        [field: SerializeField] internal bool IsSingle { get; private set; } = true;

        [SerializeField] private ButtonComponent _closeButton;
        [SerializeField] private float _animationDuration = 0.1f;

        internal Observable<Unit> OnHidePopUp => _onHidePopUpSubject.AsObservable();

        private readonly Subject<Unit> _onHidePopUpSubject = new();

        internal override void BaseInit()
        {
            if (_closeButton)
                _closeButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.HideAsync().Forget());
        }

        internal override async UniTask ShowAsync()
        {
            await Tween.Alpha(CanvasGroup, 1f, _animationDuration)
                .OnComplete(this, windowBase => windowBase.CanvasGroup.Show());
        }

        internal override async UniTask HideAsync()
        {
            await Tween.Alpha(CanvasGroup, 0f, _animationDuration)
                .OnComplete(this, windowBase => windowBase.HideImmediately());
        }

        internal override void HideImmediately()
        {
            base.HideImmediately();

            _onHidePopUpSubject.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            _onHidePopUpSubject?.Dispose();
        }
    }
}