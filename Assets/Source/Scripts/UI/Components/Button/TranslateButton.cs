using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.Translator;
using Source.Scripts.UI.Components.Accordion;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components.Button
{
    internal sealed class TranslateButton : MonoBehaviour
    {
        [SerializeField] private ThemeButton _buttonComponent;
        [SerializeField] private TextMeshProUGUI _textsToTranslate;
        [SerializeField] private TextMeshProUGUI _translatedText;

        [SerializeField] private AccordionComponent _translationAccordion;

        private bool _wasTranslated;

        private ITranslator _translator;

        [Inject]
        internal void Inject(ITranslator translator)
        {
            _translator = translator;
        }

        private void Awake()
        {
            _buttonComponent.OnClickAsObservable()
                .Where(this, static self => self._wasTranslated is false)
                .SubscribeUntilDestroy(this, static self => self.TranslateText().Forget());

            _translator.IsAvailable.SubscribeToInteractableUntilDestroy(_buttonComponent);
        }

        private async UniTask TranslateText()
        {
            var translateTextAsync =
                await _translator.TranslateTextAsync(_textsToTranslate.text, destroyCancellationToken);

            _translatedText.text = translateTextAsync;
            _wasTranslated = true;
            _translationAccordion.SetReady(VisibilityState.Visible);
        }
    }
}