using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Configs
{
    internal sealed class AppConfig : ScriptableObject, IAppConfig
    {
        [field: SerializeField]
        public EnumArray<LearningState, TrackConditionType> TrackConditionTypes { get; private set; } =
            new(EnumMode.Default);

        [field: SerializeField]
        public EnumArray<LearningState, LearningState> SuccessTransitionMap { get; private set; } =
            new(EnumMode.Default);

        [field: SerializeField]
        public EnumArray<LearningState, LearningState> FailureTransitionMap { get; private set; } =
            new(EnumMode.Default);

        private EnumArray<PracticeState, LearningState> _targetStateForLearnedWords;
        [field: SerializeField]
        public EnumArray<PracticeState, LearningState[]> TargetLearningStatesForPractice { get; private set; }
            = new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<PracticeState, LearningState> TargetStateForLearnedWords { get; private set; }
            = new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<PracticeState, ModuleType> PracticeToModuleType { get; private set; }
            = new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<LanguageType, SystemLanguage[]> SupportedLanguages { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField] public LearningState[] CooldownStates { get; private set; }
        [field: SerializeField] public PracticeState OnboardingPracticeState { get; private set; }
        [field: SerializeField] public SystemLanguage DefaultNativeLanguage { get; private set; }
        [field: SerializeField] public SystemLanguage DefaultLearningLanguage { get; private set; }
        [field: SerializeField] public LearningState LearningStateForDailyGoal { get; private set; }
    }
}