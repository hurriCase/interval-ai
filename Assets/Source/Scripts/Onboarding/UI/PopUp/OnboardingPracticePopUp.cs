using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Onboarding.UI.PopUp.WordPractice;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.PopUp
{
    internal sealed class OnboardingPracticePopUp : PopUpBase
    {
        [SerializeField] private OnboardingPracticeBehaviour _onboardingPracticeBehaviour;

        [SerializeField] private TextMeshProUGUI _messageText;

        [SerializeField] private Transform _imageTint;
        [SerializeField] private List<WordPracticeStepData> _wordPracticeSteps;

        private ButtonComponent _continueButton;
        private GameObject _placeholderObject;

        private int _currentStepIndex;

        internal override void Init()
        {
            _onboardingPracticeBehaviour.Init();

            foreach (var wordPracticeStepData in _wordPracticeSteps)
                wordPracticeStepData.Init(_imageTint);

            UpdateView();
        }

        private void SwitchWordPracticeStep()
        {
            _wordPracticeSteps[_currentStepIndex].RestoreButton();

            _currentStepIndex++;

            if (_currentStepIndex >= _wordPracticeSteps.Count)
            {
                Hide();
                return;
            }

            UpdateView();
        }

        private void UpdateView()
        {
            var currentStep = _wordPracticeSteps[_currentStepIndex];

            currentStep.SwitchButton.OnClickAsObservable()
                .Subscribe(this, (_, self) => self.SwitchWordPracticeStep())
                .RegisterTo(destroyCancellationToken);

            _messageText.text = currentStep.LocalizationKey.GetLocalization();

            _wordPracticeSteps[_currentStepIndex].ApplyHighlightEffect();
        }

        private void OnDestroy()
        {
            _wordPracticeSteps[_currentStepIndex].RestoreButton();
        }
    }
}