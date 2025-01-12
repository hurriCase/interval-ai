using System;
using Client.Scripts.Core.Audio;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.Patterns.DI.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Scripts.UI.Base
{
    internal sealed class ButtonComponent : InjectableBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _clickSound;
        [Inject] private IAudioController _audioController;
        [Inject] private ICloudRepository _cloudRepository;

        internal event Action OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            _audioController.PlayEffect(_clickSound);

            OnClick?.Invoke();
        }
    }
}