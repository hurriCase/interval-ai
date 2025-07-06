using System;
using R3;
using Source.Scripts.UI.Selectables;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class PopUpBase : WindowBase<PopUpType>
    {
        [SerializeField] private ButtonComponent _closeButton;

        internal event Action OnHidePopUp;

        internal override void BaseInit()
        {
            _closeButton.OnClickAsObservable().Subscribe(this, (_, popUp) => popUp.Hide()).AddTo(this);
        }

        internal override void HideImmediately()
        {
            base.HideImmediately();

            OnHidePopUp?.Invoke();
        }
    }
}