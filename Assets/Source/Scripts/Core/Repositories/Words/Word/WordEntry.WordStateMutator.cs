using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Constants;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal sealed partial class WordEntry
    {
        [Preserve]
        internal sealed class WordStateMutator : IWordStateMutator
        {
            private readonly IPracticeSettingsRepository _practiceSettingsRepository;
            private readonly IProgressRepository _progressRepository;
            private readonly IWordsRepository _wordsRepository;
            private readonly IAppConfig _appConfig;

            [Preserve]
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
                using var saveScope = _wordsRepository.CreateSaveScope();

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

                if (success is false)
                    return;

                IncrementProgress(word, TrackConditionType.OnExit);

                TryAdvanceCooldown(word);
            }

            public void HideWord(WordEntry word)
            {
                using var saveScope = _wordsRepository.CreateSaveScope();

                word.IsHidden = true;

                _wordsRepository.RemoveHiddenWord(word);
            }

            public void ResetWord(WordEntry word)
            {
                using var saveScope = _wordsRepository.CreateSaveScope();

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

                using var saveScope = _wordsRepository.CreateSaveScope();

                var cooldownData = _practiceSettingsRepository.RepetitionByCooldown.Value[word.ReviewCount];
                word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);
            }

            private void IncrementProgress(WordEntry word, TrackConditionType targetTrackCondition)
            {
                var trackCondition = _appConfig.TrackConditionTypes[word.LearningState];

                if (trackCondition != targetTrackCondition)
                    return;

                _progressRepository.IncrementDailyProgress(word.LearningState, Date.Today);
            }

            private void HandleReview(WordEntry word, bool success)
            {
                using var saveScope = _wordsRepository.CreateSaveScope();

                if (success is false)
                {
                    word.ReviewCount = Math.Max(0, word.ReviewCount - 1);

                    _wordsRepository.CreateSaveScope();
                    return;
                }

                word.ReviewCount++;
                if (word.ReviewCount > _practiceSettingsRepository.RepetitionByCooldown.Value.Count)
                    word.LearningState = LearningState.Studied;
            }
        }
    }
}