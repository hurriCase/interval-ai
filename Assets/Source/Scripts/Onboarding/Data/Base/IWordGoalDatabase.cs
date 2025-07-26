using System.Collections.Generic;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base
{
    internal interface IWordGoalDatabase
    {
        List<int> DefaultWordGoals { get; }
    }
}