using CustomUtils.Runtime.Attributes;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Core.Repositories.Base.Tests.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Base.Tests
{
    [CreateAssetMenu(fileName = nameof(TestConfig), menuName = nameof(TestConfig))]
    internal sealed class TestConfig : ScriptableObject, ITestConfig
    {
        [field: SerializeField] public EnumArray<LearningState, RandomInt> WordsCountByState { get; private set; }
        [field: SerializeField] public bool IsSkipOnboarding { get; private set; }
        [field: SerializeField] public bool UseTestLanguage { get; private set; }

        [field: SerializeField, HideIf(nameof(UseTestLanguage))]
        public SystemLanguage TestLanguage { get; private set; }
    }
}