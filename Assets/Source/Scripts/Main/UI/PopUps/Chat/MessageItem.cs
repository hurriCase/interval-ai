using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.LanguageDetector;
using Source.Scripts.Core.Localization.Translator;
using Source.Scripts.Core.Repositories.Settings.Base;
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

        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILanguageDetector _languageDetector;
        private ITranslator _translator;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            ILanguageDetector languageDetector,
            ITranslator translator)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _languageDetector = languageDetector;
            _translator = translator;
        }

        internal void Init(string message)
        {
            _messageText.text = message;

            AdjustMessageSize();

            _translateButton.OnClickAsObservable()
                .Where(this, self => self._wasTranslated is false)
                .SubscribeAndRegister(this, static self => self.TranslateText().Forget());
        }

        private async UniTask TranslateText()
        {
            var language = _languageDetector.DetectLanguage(_messageText.text);
            var oppositeLanguage = _languageSettingsRepository.GetOppositeLanguage(language);
            var translateTextAsync =
                await _translator.TranslateTextAsync(_messageText.text, oppositeLanguage, destroyCancellationToken);

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