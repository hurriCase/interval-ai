using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Core.Repositories.Base.Tests.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Base.Tests
{
    [CreateAssetMenu(fileName = nameof(TestDataConfig), menuName = nameof(TestDataConfig))]
    internal sealed class TestDataConfig : ScriptableObject, ITestDataConfig
    {
        [field: SerializeField] public EnumArray<LearningState, RandomInt> WordsCountByState { get; private set; }
    }
}