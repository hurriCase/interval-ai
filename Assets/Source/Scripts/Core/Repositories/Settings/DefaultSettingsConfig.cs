using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings
{
    internal sealed class DefaultSettingsConfig : ScriptableObject, IDefaultSettingsConfig
    {
        [field: SerializeField] public List<CooldownByDate> Cooldowns { get; private set; }
        [field: SerializeField] public int DailyGoal { get; private set; }
    }
}