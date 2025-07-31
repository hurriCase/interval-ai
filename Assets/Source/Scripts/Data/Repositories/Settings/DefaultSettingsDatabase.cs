using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Settings
{
    [CreateAssetMenu(fileName = nameof(DefaultSettingsDatabase), menuName = nameof(DefaultSettingsDatabase))]
    internal sealed class DefaultSettingsDatabase : ScriptableObject, IDefaultSettingsDatabase
    {
        [field: SerializeField] public List<CooldownByDate> Cooldowns { get; private set; }
        [field: SerializeField] public int DailyGoal { get; private set; }
    }
}