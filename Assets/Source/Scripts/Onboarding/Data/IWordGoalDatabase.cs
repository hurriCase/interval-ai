using System.Collections.Generic;

namespace Source.Scripts.Onboarding.Data
{
    internal interface IWordGoalDatabase
    {
        List<int> DefaultWordGoals { get; }
    }
}