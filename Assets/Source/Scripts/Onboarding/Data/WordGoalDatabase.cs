using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Onboarding.Data
{
    internal sealed class WordGoalDatabase : ScriptableObject, IWordGoalDatabase
    {
        [field: SerializeField] public List<int> DefaultWordGoals { get; private set; }
    }
}