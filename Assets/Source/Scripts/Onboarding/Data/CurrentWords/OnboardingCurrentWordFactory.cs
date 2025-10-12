using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using UnityEngine.Scripting;

namespace Source.Scripts.Onboarding.Data.CurrentWords
{
    [Preserve]
    internal sealed class OnboardingCurrentWordFactory : ICurrentWordFactory
    {
        private readonly ICurrentWordService _service;

        [Preserve]
        internal OnboardingCurrentWordFactory(ICurrentWordService service)
        {
            _service = service;
        }

        public ICurrentWordService GetOrCreate(PracticeState practiceState) => _service;
    }
}