using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using TMPro;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Progress
{
    internal sealed class DailyProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProgressPercentText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressDescriptionText;
        [SerializeField] private RoundedFilledImageComponent _progressComponent;

        private ProgressDescriptionsDatabase ProgressDescriptionsDatabase => ProgressDescriptionsDatabase.Instance;
        private ProgressRepository ProgressRepository => ProgressRepository.Instance;

        internal void Init()
        {
            ProgressRepository.ProgressHistory
                .AsObservable()
                .DistinctUntilChangedBy(progress => progress)
                .Subscribe(this, (progressEntry, behaviour) => behaviour.UpdateProgress(progressEntry));

            UpdateProgress(ProgressRepository.ProgressHistory.Value);
        }

        private void UpdateProgress(Dictionary<DateTime, DailyProgress> progressHistory)
        {
            var dailyGoal = Mathf.Max(1, ProgressRepository.DailyWordsGoal.Value);
            var learnedCount = progressHistory.TryGetValue(DateTime.Now, out var dailyProgress)
                ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.CurrentlyLearning))
                : 0;

            var currentProgressRatio = learnedCount / dailyGoal;

            var displayRatio = Mathf.Min(currentProgressRatio, 1.0f);

            var currentProgressPercent = Mathf.RoundToInt(currentProgressRatio * 100);

            _currentProgressPercentText.text = $"{currentProgressPercent}%";
            _progressComponent.fillAmount = displayRatio;

            var (titleLocalization, progressLocalization, percent) =
                GetRandomDescription(DetermineProgressType(learnedCount));

            _titleText.text = titleLocalization;
            _progressDescriptionText.text = string.Format(progressLocalization, learnedCount, dailyGoal, percent);
        }

        private ProgressDescriptionType DetermineProgressType(int learnedWordCount)
        {
            if (learnedWordCount <= 0)
                return ProgressDescriptionType.Zero;

            if (learnedWordCount < ProgressDescriptionsDatabase.LowMediumTransitionRandom.RandomValue)
                return ProgressDescriptionType.Low;

            return learnedWordCount < ProgressDescriptionsDatabase.MediumHighTransitionRandom.RandomValue
                ? ProgressDescriptionType.Medium
                : ProgressDescriptionType.High;
        }

        private (string, string, int) GetRandomDescription(ProgressDescriptionType progressType)
        {
            var localizationData =
                ProgressDescriptionsDatabase.DescriptionLocalizations.AsValueEnumerable()
                    .Where(progressDescriptionData => progressDescriptionData.Type == progressType);

            if (localizationData.Count() == 0)
                return (LocalizationType.ProgressTitle.GetLocalization(),
                    LocalizationType.ProgressDescription.GetLocalization(),
                    ProgressDescriptionsDatabase.DefaultRandomPercent.RandomValue);

            var randomData = localizationData.Random();

            return (randomData.TitleKey.GetLocalization(),
                randomData.ProgressDescriptionKey.GetLocalization(),
                randomData.Percent.RandomValue);
        }
    }
}