using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Base.Tests.Base
{
    internal interface ITestDataConfig
    {
        EnumArray<LearningState, RandomInt> WordsCountByState { get; }
    }
}