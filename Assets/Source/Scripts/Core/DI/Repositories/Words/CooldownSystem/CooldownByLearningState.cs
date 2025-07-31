using System;
using Source.Scripts.Core.DI.Repositories.Words.Base;

namespace Source.Scripts.Core.DI.Repositories.Words.CooldownSystem
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