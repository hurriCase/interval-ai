using System;
using Client.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Scripts.UI
{
    internal sealed class ButtonComponent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _clickSound;

        internal event Action OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.Instance.PlayEffect(_clickSound);

            OnClick?.Invoke();
        }
    }
}