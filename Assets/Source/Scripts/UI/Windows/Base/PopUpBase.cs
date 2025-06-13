using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.Base
{
    internal sealed class PopUpBase : WindowBase<PopUpType>
    {
        [SerializeField] private Button _closeButton;

        internal event Action OnHidePopUp;

        internal override void Init()
        {
            _closeButton.onClick.AddListener(Hide);
        }

        protected override void OnHideComplete()
        {
            base.OnHideComplete();

            OnHidePopUp?.Invoke();
        }
    }
}