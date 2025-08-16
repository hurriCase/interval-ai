using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface IDefaultSettingsConfig
    {
        List<CooldownByDate> Cooldowns { get; }
        int DailyGoal { get; }
        SystemLanguage NativeLanguage { get; }
        SystemLanguage LearningLanguage { get; }
        LanguageType FirstShowPractice { get; }
        LanguageType CardLearnPractice { get; }
        LanguageType CardReviewPractice { get; }
        WordReviewSourceType WordReviewSourceType { get; }
    }
}