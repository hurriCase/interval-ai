using System.Collections.Generic;

namespace Source.Scripts.Data.Repositories.Progress.Base
{
    internal interface IWordGoalDatabase
    {
        List<int> DefaultWordGoals { get; }
    }
}