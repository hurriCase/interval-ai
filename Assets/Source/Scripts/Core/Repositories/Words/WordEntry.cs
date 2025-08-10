using System;
using MemoryPack;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Sprites;
using VContainer;

namespace Source.Scripts.Core.Repositories.Words
{
    [MemoryPackable]
    internal sealed partial class WordEntry
    {
        public int CategoryId { get; set; }
        public LearningState LearningState { get; private set; }
        public string NativeWord { get; set; }
        public string LearningWord { get; set; }
        public string NativeExample { get; set; }
        public string LearningExample { get; set; }
        public string Transcription { get; set; }
        public CachedSprite DescriptiveImage { get; set; }
        public int RepetitionCount { get; private set; }
        public bool IsHidden { get; set; }
        public DateTime Cooldown { get; set; } = DateTime.MinValue;

        private IProgressRepository _progressRepository;
        private ISettingsRepository _settingsRepository;
        private IAppConfig _appConfig;

        [Inject]
        internal void Inject(
            IProgressRepository progressRepository,
            ISettingsRepository settingsRepository,
            IAppConfig appConfig)
        {
            _progressRepository = progressRepository;
            _settingsRepository = settingsRepository;
            _appConfig = appConfig;
        }

        internal void AdvanceLearningState(bool success)
        {
            if (success)
                IncrementProgress(TrackConditionType.OnEnter);

            if (LearningState == LearningState.Review)
                HandleReview(success);

            var transitionMap = success
                ? _appConfig.SuccessTransitionMap
                : _appConfig.FailureTransitionMap;

            LearningState = transitionMap[LearningState];

            if (!success)
                return;

            IncrementProgress(TrackConditionType.OnExit);
        }

        private void IncrementProgress(TrackConditionType targetTrackCondition)
        {
            var trackCondition = _appConfig.TrackConditionTypes[LearningState];

            if (trackCondition == targetTrackCondition)
                _progressRepository.IncrementDailyProgress(LearningState, DateTime.Now);
        }

        private void HandleReview(bool success)
        {
            if (success is false)
            {
                RepetitionCount = Math.Max(0, RepetitionCount - 1);
                return;
            }

            RepetitionCount++;
            if (RepetitionCount < _settingsRepository.RepetitionByCooldown.Value.Count)
                return;

            LearningState = LearningState.Studied;
        }
    }
}