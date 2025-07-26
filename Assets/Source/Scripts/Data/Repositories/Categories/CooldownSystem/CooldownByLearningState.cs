using System;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Data.Repositories.Categories.CooldownSystem
{
    internal readonly struct CooldownByLearningState
    {
        internal LearningState State { get; }
        internal DateTime CurrentTime { get; }

        internal CooldownByLearningState(LearningState state, DateTime currentTime)
        {
            State = state;
            CurrentTime = currentTime;
        }
    }
}