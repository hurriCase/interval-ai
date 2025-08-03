using Cysharp.Text;
using Source.Scripts.Core.Repositories.Settings.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Behaviours
{
    internal sealed class WordCongratulationBehaviour : StepBehaviourBase
    {
        [SerializeField] private TextMeshProUGUI _wordCountText;

        [Inject] private ISettingsRepository _settingsRepository;

        private const int DayInMonths = 30;

        internal override void Init()
        {
            var learnedWordCount = _settingsRepository.DailyGoal.Value * DayInMonths;

            _wordCountText.SetTextFormat("{0}{1}", learnedWordCount, "!");
        }
    }
}