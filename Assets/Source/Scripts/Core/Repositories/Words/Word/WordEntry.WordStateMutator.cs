using System;
using System.Collections.Generic;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal sealed partial class WordEntry
    {
        internal sealed class WordStateMutator : IWordStateMutator
        {
            private readonly IPracticeSettingsRepository _practiceSettingsRepository;
            private readonly IProgressRepository _progressRepository;
            private readonly IWordsRepository _wordsRepository;
            private readonly IAppConfig _appConfig;

            internal WordStateMutator(
                IProgressRepository progressRepository,
                IPracticeSettingsRepository practiceSettingsRepository,
                IWordsRepository wordsRepository,
                IAppConfig appConfig)
            {
                _progressRepository = progressRepository;
                _practiceSettingsRepository = practiceSettingsRepository;
                _wordsRepository = wordsRepository;
                _appConfig = appConfig;
            }

            public void AdvanceLearningState(WordEntry word, bool success)
            {
                var oldState = word.LearningState;

                if (success)
                    IncrementProgress(word, TrackConditionType.OnEnter);

                if (word.LearningState == LearningState.Review)
                    HandleReview(word, success);

                var transitionMap = success
                    ? _appConfig.SuccessTransitionMap
                    : _appConfig.FailureTransitionMap;

                word.LearningState = transitionMap[word.LearningState];

                if (oldState != word.LearningState)
                    _wordsRepository.OnWordStateChanged(word, oldState, word.LearningState);

                if (!success)
                    return;

                IncrementProgress(word, TrackConditionType.OnExit);

                TryAdvanceCooldown(word);
            }

            public void HideWord(WordEntry word)
            {
                word.IsHidden = true;

                _wordsRepository.RemoveHiddenWord(word);
            }

            public void ResetWord(WordEntry word)
            {
                word.LearningState = LearningState.Review;
                word.ReviewCount = 0;
                word.Cooldown = DateTime.MinValue;
                word.IsHidden = false;
            }

            public void SetCategories(WordEntry word, List<int> categoryIds) => word.CategoryIds = categoryIds;

            private void TryAdvanceCooldown(WordEntry word)
            {
                if (word.LearningState != LearningState.Review)
                    return;

                var cooldownData = _practiceSettingsRepository.RepetitionByCooldown.Value[word.ReviewCount];
                word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);
            }

            private void IncrementProgress(WordEntry word, TrackConditionType targetTrackCondition)
            {
                var trackCondition = _appConfig.TrackConditionTypes[word.LearningState];

                if (trackCondition == targetTrackCondition)
                    _progressRepository.IncrementDailyProgress(word.LearningState, DateTime.Now);
            }

            private void HandleReview(WordEntry word, bool success)
            {
                if (success is false)
                {
                    word.ReviewCount = Math.Max(0, word.ReviewCount - 1);
                    return;
                }

                word.ReviewCount++;
                if (word.ReviewCount < _practiceSettingsRepository.RepetitionByCooldown.Value.Count)
                    return;

                word.LearningState = LearningState.Studied;
            }
        }
    }
}