using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base.PopUp
{
    internal abstract class PopUpBase : WindowBase
    {
        [SerializeField] private ButtonComponent _closeButton;
        [SerializeField] private float _animationDuration = 0.1f;

        internal Observable<Unit> OnHidePopUp => _onHidePopUpSubject.AsObservable();

        private readonly Subject<Unit> _onHidePopUpSubject = new();

        internal override void BaseInit()
        {
            if (_closeButton)
                _closeButton.OnClickAsObservable()
                    .Subscribe(this, (_, popUp) => popUp.Hide())
                    .RegisterTo(destroyCancellationToken);
        }

        internal override void Show()
        {
            Tween.Alpha(CanvasGroup, 1f, _animationDuration)
                .OnComplete(this, windowBase => windowBase.CanvasGroup.Show());
        }

        internal override void Hide()
        {
            Tween.Alpha(CanvasGroup, 0f, _animationDuration)
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