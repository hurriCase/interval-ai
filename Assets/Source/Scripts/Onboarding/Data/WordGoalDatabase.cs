using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Progress.Base;
using UnityEngine;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data
{
    internal sealed class WordGoalDatabase : ScriptableObject, IWordGoalDatabase
    {
        [field: SerializeField] public List<int> DefaultWordGoals { get; private set; }
    }
}