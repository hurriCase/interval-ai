using System;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Vocabulary
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