using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Randoms;
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
    internal sealed class ProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProgressPercentText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressDescriptionText;
        [SerializeField] private RoundedFilledImageComponent _progressComponent;

        [SerializeField] private List<ProgressDescriptionData> _progressDescriptionLocalizations;
        [SerializeField] private RandomInt _lowMediumTransitionRandom;
        [SerializeField] private RandomInt _mediumHighTransitionRandom;
        [SerializeField] private RandomInt _defaultRandomPercent = new(20, 50);

        internal void Init()
        {
            ProgressRepository.Instance.ProgressEntry
                .AsObservable()
                .DistinctUntilChangedBy(progress => progress.ProgressHistory)
                .Subscribe(this, (progressEntry, behaviour) => behaviour.UpdateProgress(progressEntry));

            UpdateProgress(ProgressRepository.Instance.ProgressEntry.Value);
        }

        private void UpdateProgress(ProgressEntry progressEntry)
        {
            var dailyGoal = Mathf.Max(1, progressEntry.DailyWordsGoal);
            var learnedCount = progressEntry.ProgressHistory.TryGetValue(DateTime.Now, out var dailyProgress)
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

            if (learnedWordCount < _lowMediumTransitionRandom.RandomValue)
                return ProgressDescriptionType.Low;

            return learnedWordCount < _mediumHighTransitionRandom.RandomValue
                ? ProgressDescriptionType.Medium
                : ProgressDescriptionType.High;
        }

        private (string, string, int) GetRandomDescription(ProgressDescriptionType progressType)
        {
            var localizationData = _progressDescriptionLocalizations.AsValueEnumerable()
                .Where(progressDescriptionData => progressDescriptionData.Type == progressType);

            if (localizationData.Count() == 0)
                return (LocalizationType.ProgressTitle.GetLocalization(),
                    LocalizationType.ProgressDescription.GetLocalization(),
                    _defaultRandomPercent.RandomValue);

            var randomData = localizationData.Random();

            return (randomData.TitleKey.GetLocalization(),
                randomData.ProgressDescriptionKey.GetLocalization(),
                randomData.Percent.RandomValue);
        }
    }
}