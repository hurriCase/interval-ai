using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Onboarding.UI.Screen.Behaviours;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Screen
{
    internal sealed class OnboardingInputScreen : ScreenBase
    {
        [SerializeField] private ButtonComponent _continueButton;

        [SerializeField] private List<StepBehaviourBase> _inputOnboardingSteps;

        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IStatisticsRepository _statisticsRepository;
        [Inject] private ISceneReferences _sceneReferences;

        private int _currentStepIndex;
        private bool _isWordPracticeOnBoarding;

        internal override void Init()
        {
            foreach (var inputOnboardingStep in _inputOnboardingSteps)
            {
                inputOnboardingStep.SetActive(false);
                inputOnboardingStep.Init();
                inputOnboardingStep.OnContinueSubject
                    .Subscribe(this, static (_, self) => self.SwitchModule())
                    .RegisterTo(destroyCancellationToken);
            }

            SwitchSettingsStep(_currentStepIndex, true);

            _continueButton.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.SwitchModule())
                .RegisterTo(destroyCancellationToken);
        }

        private void SwitchModule()
        {
            SwitchSettingsStep(_currentStepIndex, false);

            _currentStepIndex++;

            SwitchSettingsStep(_currentStepIndex, true);
        }

        private void SwitchSettingsStep(int index, bool isActive)
        {
            if (index >= _inputOnboardingSteps.Count)
            {
                // _statisticsRepository.IsCompleteOnboarding.Value = true;
                // _sceneLoader.LoadSceneAsync(_sceneReferences.MainMenuScene.Address, destroyCancellationToken).Forget();
                return;
            }

            var inputStep = _inputOnboardingSteps[index];
            if (isActive)
                inputStep.UpdateView();
            else
                inputStep.OnContinue();

            inputStep.SetActive(isActive);
        }
    }
}