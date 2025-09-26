using System;
using System.Threading;
using R3;
using R3.Triggers;
using Source.Scripts.Onboarding.UI.OnboardingPractice.Steps.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Source.Scripts.Onboarding.UI.OnboardingPractice.Steps
{
    [Serializable]
    internal sealed class HighlightStep : PracticeStepBase
    {
        [SerializeField] private Selectable _switchButton;
        [SerializeField] private Canvas _mainCanvas;

        private Canvas _highlightCanvas;
        private GraphicRaycaster _highlightRaycaster;

        internal override void OnInit(CancellationToken cancellationToken)
        {
            disposable = _switchButton.OnPointerClickAsObservable()
                .Subscribe(switched, static (_, switchSubject) => switchSubject.OnNext(Unit.Default));

            disposable.RegisterTo(cancellationToken);
        }

        internal override void ActiveStep()
        {
            _highlightCanvas = _switchButton.gameObject.AddComponent<Canvas>();
            _highlightCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 |
                                                         AdditionalCanvasShaderChannels.TexCoord2 |
                                                         AdditionalCanvasShaderChannels.TexCoord3;
            _highlightRaycaster = _switchButton.gameObject.AddComponent<GraphicRaycaster>();
            _highlightCanvas.overrideSorting = true;
            _highlightCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _highlightCanvas.sortingOrder = _mainCanvas.sortingOrder + 1;
        }

        internal override void OnHideStep()
        {
            Object.Destroy(_highlightRaycaster);
            Object.Destroy(_highlightCanvas);
        }

        protected override void OnUpdateText(TextMeshProUGUI messageText)
        {
            //messageText.color = hintTextMapping.GetColorForState(HintTextThemeState.Highlight);
        }
    }
}