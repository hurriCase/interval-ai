using System.Collections.Generic;

namespace Source.Scripts.Data.Repositories.Settings.Base
{
    internal interface IDefaultSettingsDatabase
    {
        List<CooldownByDate> Cooldowns { get; }
        int DailyGoal { get; }
    }
}