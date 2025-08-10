using System;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Core.Repositories.Words.CooldownSystem
{
    internal readonly struct CooldownByPracticeState
    {
        internal PracticeState PracticeState { get; }
        internal DateTime CurrentTime { get; }

        internal CooldownByPracticeState(PracticeState practiceState, DateTime currentTime)
        {
            PracticeState = practiceState;
            CurrentTime = currentTime;
        }
    }
}