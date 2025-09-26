using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.Translator;
using Source.Scripts.UI.Components.Accordion;
using Source.Scripts.UI.Components.Animation.Base;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Chat
{
    internal sealed class MessageItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private TextMeshProUGUI _messageText;

        [SerializeField] private ButtonComponent _pinButton;

        [SerializeField] private ButtonComponent _translateButton;
        [SerializeField] private AccordionComponent _translationAccordion;
        [SerializeField] private TextMeshProUGUI _translatedText;

        [SerializeField] private float _minWidth;
        [SerializeField] private float _maxWidth;
        [SerializeField] private float _otherContentWidth;

        private bool _wasTranslated;

        private ITranslator _translator;

        [Inject]
        internal void Inject(ITranslator translator)
        {
            _translator = translator;
        }

        internal void Init(string message)
        {
            _messageText.text = message;

            AdjustMessageSize();

            _translateButton.OnClickAsObservable()
                .Where(this, self => self._wasTranslated is false)
                .SubscribeUntilDestroy(this, static self => self.TranslateText().Forget());
        }

        private async UniTask TranslateText()
        {
            var translateTextAsync =
                await _translator.TranslateTextAsync(_messageText.text, destroyCancellationToken);

            _translatedText.text = translateTextAsync;
            _wasTranslated = true;
            _translationAccordion.SetReady(VisibilityState.Visible);
        }

        private void AdjustMessageSize()
        {
            var contentWidth = _messageText.preferredWidth + _otherContentWidth;
            var targetWidth = Mathf.Clamp(contentWidth, _minWidth, _maxWidth);
            _contentContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        }
    }
}