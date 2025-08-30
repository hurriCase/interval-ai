using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Scenes;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput
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
                inputOnboardingStep.OnContinueSubject.SubscribeAndRegister(this, static self => self.SwitchModule());
            }

            SwitchSettingsStep(_currentStepIndex, true);

            _continueButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.SwitchModule());
        }

        private void SwitchModule()
        {
            var nextSTep = _currentStepIndex + 1;
            if (nextSTep >= _inputOnboardingSteps.Count)
            {
                _statisticsRepository.IsCompleteOnboarding.Value = true;
                _sceneLoader.LoadSceneAsync(_sceneReferences.MainMenuScene.Address, CancellationToken.None).Forget();
                return;
            }

            SwitchSettingsStep(_currentStepIndex, false);

            _currentStepIndex++;

            SwitchSettingsStep(_currentStepIndex, true);
        }

        private void SwitchSettingsStep(int index, bool isActive)
        {
            var inputStep = _inputOnboardingSteps[index];
            if (isActive)
                inputStep.UpdateView();
            else
                inputStep.OnContinue();

            inputStep.SetActive(isActive);
        }
    }
}