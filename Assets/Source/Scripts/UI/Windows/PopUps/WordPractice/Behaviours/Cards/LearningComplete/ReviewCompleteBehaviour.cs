using System;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class ReviewCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private TextMeshProUGUI _remainingTimeText;

        protected override void OnInit()
        {
            VocabularyRepository.Instance.OnAvailabilityTimeUpdate
                .Where(update => update.state == LearningState.Repeatable)
                .Subscribe(this, static (tuple, card) => card.UpdateTime(tuple.currentTime))
                .RegisterTo(destroyCancellationToken);

            exitButton.OnClickAsObservable()
                .Subscribe(static _ => WindowsController.Instance.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateTime(DateTime currentTime)
        {
            _remainingTimeText.text =
                string.Format(LocalizationController.Localize("ui.word-practice.cooldown-until-new-words"),
                    FormatTimeUntil(currentTime));
        }

        private static string FormatTimeUntil(DateTime targetTime)
        {
            var timeSpan = targetTime - DateTime.Now;

            switch (timeSpan.TotalSeconds)
            {
                case <= 0:
                    return "Available now";
                case < 60:
                    return $"{(int)timeSpan.TotalSeconds}s";
            }

            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m";

            if (timeSpan.TotalHours < 24)
            {
                var hours = (int)timeSpan.TotalHours;
                var minutes = timeSpan.Minutes;

                return minutes > 5 ? $"{hours}h {minutes}m" : $"{hours}h";
            }

            if ((timeSpan.TotalDays < 7) is false)
                return $"{(int)timeSpan.TotalDays}d";
            {
                var days = (int)timeSpan.TotalDays;
                var hours = timeSpan.Hours;

                return hours > 2 ? $"{days}d {hours}h" : $"{days}d";
            }
        }
    }
}