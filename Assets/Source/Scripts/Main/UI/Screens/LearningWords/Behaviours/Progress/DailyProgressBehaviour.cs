using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.FilledImage;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.Data.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    internal sealed class DailyProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProgressPercentText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressDescriptionText;
        [SerializeField] private RoundedFilledImage _progressComponent;

        private IProgressDescriptionsDatabase _progressDescriptionsDatabase;
        private IPracticeSettingsRepository _practiceSettingsRepository;
        private IProgressRepository _progressRepository;

        [Inject]
        internal void Inject(
            IProgressDescriptionsDatabase progressDescriptionsDatabase,
            IPracticeSettingsRepository practiceSettingsRepository,
            IProgressRepository progressRepository)
        {
            _progressDescriptionsDatabase = progressDescriptionsDatabase;
            _practiceSettingsRepository = practiceSettingsRepository;
            _progressRepository = progressRepository;
        }

        internal void Init()
        {
            _progressRepository.TodayProgress
                .DistinctUntilChangedBy(static progress => progress)
                .SubscribeUntilDestroy(this, static self => self.UpdateProgress());

            LocalizationController.Language.SubscribeUntilDestroy(this, static self => self.UpdateProgress());
        }

        private void UpdateProgress()
        {
            var learnedCount = _progressRepository.TodayProgress.CurrentValue
                .GetProgressCountData(LearningState.CurrentlyLearning);

            var dailyGoal = Mathf.Max(1, _practiceSettingsRepository.DailyGoal.Value);
            var progressRatio = (float)learnedCount / dailyGoal;

            UpdateProgressUI(progressRatio);
            UpdateDescriptionUI(learnedCount, dailyGoal);
        }

        private void UpdateProgressUI(float progressRatio)
        {
            progressRatio = Mathf.Min(progressRatio, 1.0f);
            _currentProgressPercentText.SetTextFormat("{0:0%}", progressRatio);
            _progressComponent.fillAmount = progressRatio;
        }

        private void UpdateDescriptionUI(int learnedCount, int dailyGoal)
        {
            var progressType = DetermineProgressType(learnedCount);
            var descriptionData = GetDescriptionData(progressType);

            _titleText.text = descriptionData.Title;
            _progressDescriptionText.SetTextFormat(descriptionData.Description,
                learnedCount, dailyGoal, descriptionData.Percent);
        }

        private ProgressDescriptionType DetermineProgressType(int learnedCount)
        {
            if (learnedCount <= 0)
                return ProgressDescriptionType.Zero;

            if (learnedCount < _progressDescriptionsDatabase.LowMediumTransitionRandom.RandomValue)
                return ProgressDescriptionType.Low;

            return learnedCount < _progressDescriptionsDatabase.MediumHighTransitionRandom.RandomValue
                ? ProgressDescriptionType.Medium
                : ProgressDescriptionType.High;
        }

        private DescriptionData GetDescriptionData(ProgressDescriptionType progressType)
        {
            var description = _progressDescriptionsDatabase.Descriptions[progressType];
            var randomData = description.ProgressLocalizationData.Random();
            return new DescriptionData(randomData, description.Percent.RandomValue);
        }
    }
}