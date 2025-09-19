using System;
using System.Collections.Generic;
using System.Threading;
using R3;
using R3.Triggers;
using Source.Scripts.Onboarding.UI.OnboardingPractice.Steps.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Onboarding.UI.OnboardingPractice.Steps
{
    [Serializable]
    internal sealed class PlainStep : PracticeStepBase
    {
        [SerializeField] private List<Selectable> _switchButtons;
        [SerializeField] private GameObject _tintObject;

        internal override void OnInit(CancellationToken cancellationToken)
        {
            var builder = Disposable.CreateBuilder();
            foreach (var buttonComponent in _switchButtons)
            {
                buttonComponent.OnPointerClickAsObservable()
                    .Subscribe(switchSubject, static (_, switchSubject) => switchSubject.OnNext(Unit.Default))
                    .AddTo(ref builder);
            }

            disposable = builder.Build();
            disposable.RegisterTo(cancellationToken);
        }

        internal override void ActiveStep() => _tintObject.SetActive(false);

        protected override void OnUpdateText(TextMeshProUGUI messageText)
        {
            messageText.color = hintTextMapping.GetColorForState(HintTextThemeState.Plain);
        }
    }
}