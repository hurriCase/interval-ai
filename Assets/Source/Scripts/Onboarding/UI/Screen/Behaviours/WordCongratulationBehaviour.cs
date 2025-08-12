using Cysharp.Text;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Base.PopUp;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours
{
    internal sealed class WordCongratulationBehaviour : StepBehaviourBase
    {
        [SerializeField] private TextMeshProUGUI _wordCountText;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private IWindowsController _windowsController;

        private const int DayInMonths = 30;

        internal override void UpdateView()
        {
            var learnedWordCount = _settingsRepository.DailyGoal.Value * DayInMonths;

            _wordCountText.SetTextFormat("{0}{1}", learnedWordCount, "!");
        }

        internal override void OnContinue()
        {
            _windowsController.OpenPopUpByType(PopUpType.OnboardingPractice);
        }
    }
}