using System.Collections.Generic;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface IPracticeSettingsRepository
    {
        PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        PersistentReactiveProperty<int> DailyGoal { get; }
        PersistentReactiveProperty<WordReviewSourceType> WordReviewSourceType { get; }
    }
}