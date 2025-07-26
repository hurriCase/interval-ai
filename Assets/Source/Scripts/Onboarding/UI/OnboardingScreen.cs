using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI.Behaviours;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI
{
    internal sealed class OnboardingScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private ButtonComponent _continueButton;

        [SerializeField] private List<StepBehaviourBase> _inputOnboardingSteps;

        private int _currentStepIndex;

        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IUserRepository _userRepository;
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
                .Subscribe(this, static (_, screen) =>
                {
                    screen.SwitchStep(screen._currentStepIndex, false);
                    screen._currentStepIndex++;
                    screen.SwitchStep(screen._currentStepIndex, true);
                })
                .RegisterTo(destroyCancellationToken);
        }

        private void SwitchStep(int index, bool isActive)
        {
            if (index >= _inputOnboardingSteps.Count)
            {
                _userRepository.IsCompleteOnboarding.Value = true;
                _sceneLoader.LoadSceneAsync(_sceneReferences.MainMenuScene.Address, CancellationToken.None)
                    .Forget();
                return;
            }

            _inputOnboardingSteps[index].SetActive(isActive);

            if (isActive)
                _titleText.text = _inputOnboardingSteps[index].TitleLocalizationKey.GetLocalization();
        }
    }
}