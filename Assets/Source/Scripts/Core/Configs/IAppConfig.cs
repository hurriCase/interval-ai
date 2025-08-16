using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Configs
{
    internal interface IAppConfig
    {
        EnumArray<LearningState, TrackConditionType> TrackConditionTypes { get; }
        EnumArray<LearningState, LearningState> SuccessTransitionMap { get; }
        EnumArray<LearningState, LearningState> FailureTransitionMap { get; }
        EnumArray<PracticeState, LearningState[]> TargetLearningStatesForPractice { get; }
        EnumArray<PracticeState, LearningState> TargetStateForLearnedWords { get; }
        EnumArray<PracticeState, ModuleType> PracticeToModuleType { get; }
        EnumArray<LanguageType, SystemLanguage[]> SupportedLanguages { get; }
        LearningState[] CooldownStates { get; }
        PracticeState OnboardingPracticeState { get; }
        LearningState LearningStateForDailyGoal { get; }
    }
}