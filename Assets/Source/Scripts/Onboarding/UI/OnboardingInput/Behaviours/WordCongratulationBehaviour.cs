using Cysharp.Text;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Onboarding.UI.Base;
using Source.Scripts.Onboarding.UI.OnboardingPractice;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal sealed class WordCongratulationBehaviour : StepBehaviourBase
    {
        [SerializeField] private ModuleType _moduleType;
        [SerializeField] private TextMeshProUGUI _wordCountText;

        [Inject] private IPracticeSettingsRepository _practiceSettingsRepository;
        [Inject] private IWindowsController _windowsController;

        private const int DayInMonths = 30;

        internal override void UpdateView()
        {
            var learnedWordCount = _practiceSettingsRepository.DailyGoal.Value * DayInMonths;

            _wordCountText.SetTextFormat("{0}{1}", learnedWordCount, "!");
        }

        internal override void OnContinue()
        {
            var onboardingPracticePopUp = _windowsController.OpenPopUp<OnboardingPracticePopUp>();
            onboardingPracticePopUp.SwitchStep(_moduleType);
        }
    }
}