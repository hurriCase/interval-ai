using Source.Scripts.Core.Configs;
using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;

namespace Source.Scripts.Main.Data.CurrentWord
{
    internal sealed class MainCurrentWordFactory : StateFactoryBase<PracticeState, ICurrentWordService>,
        ICurrentWordFactory
    {
        private readonly IProgressRepository _progressRepository;
        private readonly IWordsRepository _wordsRepository;
        private readonly IAppConfig _appConfig;

        internal MainCurrentWordFactory(
            IProgressRepository progressRepository,
            IWordsRepository wordsRepository,
            IAppConfig appConfig)
        {
            _progressRepository = progressRepository;
            _wordsRepository = wordsRepository;
            _appConfig = appConfig;
        }

        protected override ICurrentWordService CreateService(PracticeState practiceState) =>
            new MainCurrentWordService(_progressRepository, _wordsRepository, _appConfig, practiceState);
    }
}