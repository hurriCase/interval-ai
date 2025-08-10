using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Base.Tests.Base
{
    internal interface ITestConfig
    {
        EnumArray<LearningState, RandomInt> WordsCountByState { get; }
        public bool IsSkipOnboarding { get; }
        public bool UseTestLanguage { get; }
        public SystemLanguage TestLanguage { get; }
    }
}