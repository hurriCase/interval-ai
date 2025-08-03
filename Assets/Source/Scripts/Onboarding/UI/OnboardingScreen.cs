using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Onboarding.UI.Behaviours;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI
{
    internal sealed class OnboardingScreen : ScreenBase
    {
        [SerializeField] private ButtonComponent _continueButton;

        [SerializeField] private List<StepBehaviourBase> _inputOnboardingSteps;

        private int _currentStepIndex;

        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IStatisticsRepository _statisticsRepository;
        [Inject] private ISceneReferences _sceneReferences;

        internal override void Init()
        {
            foreach (var inputOnboardingStep in _inputOnboardingSteps)
            {
                inputOnboardingStep.Init();
                inputOnboardingStep.SetActive(false);
            }

            SwitchStep(_currentStepIndex, true);

            _continueButton.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.SwitchModule())
                .RegisterTo(destroyCancellationToken);
        }

        private void SwitchModule()
        {
            SwitchStep(_currentStepIndex, false);

            _inputOnboardingSteps[_currentStepIndex].OnContinue();

            _currentStepIndex++;

            SwitchStep(_currentStepIndex, true);
        }

        private void SwitchStep(int index, bool isActive)
        {
            if (index >= _inputOnboardingSteps.Count)
            {
                _statisticsRepository.IsCompleteOnboarding.Value = true;
                _sceneLoader.LoadSceneAsync(_sceneReferences.MainMenuScene.Address, CancellationToken.None)
                    .Forget();
                return;
            }

            _inputOnboardingSteps[index].SetActive(isActive);
        }
    }
}