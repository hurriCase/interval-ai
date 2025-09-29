using System;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.Theme;
using R3;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.OnboardingPractice.Steps.Base
{
    [Serializable]
    internal abstract class PracticeStepBase : IDisposable
    {
        [field: SerializeField] internal bool IsTransition { get; private set; }

        [SerializeField] private string _localizationKey;
        [SerializeField] private float _messagePoxY;

        internal Observable<Unit> OnSwitched => switched;
        protected Subject<Unit> switched = new();

        protected IDisposable disposable;
        protected HintTextMapping hintTextMapping;

        internal void Init(HintTextMapping hintTextMapping, CancellationToken cancellationToken)
        {
            this.hintTextMapping = hintTextMapping;

            OnInit(cancellationToken);
        }

        internal abstract void OnInit(CancellationToken cancellationToken);
        internal abstract void ActiveStep();
        internal virtual void OnHideStep() { }

        internal void HideStep()
        {
            OnHideStep();
            Dispose();
        }

        internal void UpdateText(TextMeshProUGUI messageText, ThemeComponent themeComponent)
        {
            var vector2 = messageText.rectTransform.anchoredPosition;
            vector2.y = _messagePoxY;
            messageText.rectTransform.anchoredPosition = vector2;

            messageText.text = _localizationKey.GetLocalization();

            OnUpdateText(themeComponent);
        }

        protected abstract void OnUpdateText(ThemeComponent themeComponent);

        public void Dispose()
        {
            switched.Dispose();
            disposable.Dispose();
        }
    }
}