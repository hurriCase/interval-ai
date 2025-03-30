using System;
using Client.Scripts.Core.Audio;
using Client.Scripts.DB.DataRepositories.Cloud;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Scripts.UI.Base.Button
{
    internal sealed class ButtonComponent : InjectableBehaviour, IPointerClickHandler
    {
        [Inject] private IAudioController _audioController;
        [Inject] private ICloudRepository _cloudRepository;

        [SerializeField] private AudioClip _clickSound;

        internal event Action OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            _audioController.PlayEffect(_clickSound);

            OnClick?.Invoke();
        }
    }
}