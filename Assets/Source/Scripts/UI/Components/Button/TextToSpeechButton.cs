using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Audio.TextToSpeech;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components.Button
{
    internal sealed class TextToSpeechButton : MonoBehaviour
    {
        [SerializeField] private ThemeButton _buttonComponent;
        [SerializeField] private TextMeshProUGUI[] _textsToSynthesize;

        private ITextToSpeech _textToSpeech;

        [Inject]
        internal void Inject(ITextToSpeech textToSpeech)
        {
            _textToSpeech = textToSpeech;
        }

        private void Awake()
        {
            _buttonComponent.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.SendAudio());

            _textToSpeech.IsAvailable.SubscribeToInteractableUntilDestroy(_buttonComponent);
        }

        private void SendAudio()
        {
            using var builder = ZString.CreateStringBuilder();

            foreach (var text in _textsToSynthesize)
                builder.AppendLine(text.text);

            _textToSpeech.SpeechTextAsync(builder.ToString(), destroyCancellationToken);
        }
    }
}