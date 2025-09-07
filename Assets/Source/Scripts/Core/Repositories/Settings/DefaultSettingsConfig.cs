using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings
{
    internal sealed class DefaultSettingsConfig : ScriptableObject, IDefaultSettingsConfig
    {
        [field: SerializeField] public List<CooldownByDate> Cooldowns { get; private set; }
        [field: SerializeField] public int DailyGoal { get; private set; }
        [field: SerializeField] public int MaxShownExamples { get; private set; }
        [field: SerializeField] public bool IsSwipeEnabled { get; private set; }
        [field: SerializeField] public LanguageLevel LanguageLevel { get; private set; }
        [field: SerializeField] public SystemLanguage NativeLanguage { get; private set; }
        [field: SerializeField] public SystemLanguage LearningLanguage { get; private set; }
        [field: SerializeField] public LanguageType FirstShowPractice { get; private set; }
        [field: SerializeField] public LanguageType CardLearnPractice { get; private set; }
        [field: SerializeField] public LanguageType CardReviewPractice { get; private set; }
        [field: SerializeField] public WordReviewSourceType WordReviewSourceType { get; private set; }
    }
}