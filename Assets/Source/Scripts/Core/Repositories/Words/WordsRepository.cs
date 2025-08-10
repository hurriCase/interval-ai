using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;
using Source.Scripts.Core.Repositories.Words.Timer;
using ZLinq;
using Random = UnityEngine.Random;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class WordsRepository : IWordsRepository, IRepository
    {
        private readonly PersistentReactiveProperty<Dictionary<int, WordEntry>> _wordEntries = new();

        public ReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> SortedWordsByState { get; }
            = new(new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer)));
        public ReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState { get; }
            = new(new EnumArray<PracticeState, WordEntry>(EnumMode.SkipFirst));

        private static readonly WordCooldownComparer _comparer = new();

        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IIdHandler<WordEntry> _idHandler;
        private readonly IAppConfig _appConfig;

        private IDisposable _disposable;

        internal WordsRepository(
            DefaultWordsDatabase defaultWordsDatabase,
            IWordsTimerService wordsTimerService,
            IIdHandler<WordEntry> idHandler,
            IAppConfig appConfig)
        {
            _defaultWordsDatabase = defaultWordsDatabase;
            _wordsTimerService = wordsTimerService;
            _idHandler = idHandler;
            _appConfig = appConfig;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                _idHandler.InitAsync(cancellationToken),

                _wordEntries.InitAsync(
                    PersistentKeys.WordEntryKey,
                    cancellationToken,
                    _idHandler.GenerateWithIds(_defaultWordsDatabase.Defaults))
            };

            await UniTask.WhenAll(initTasks);

            await _wordEntries.InitAsync(
                PersistentKeys.WordEntryKey,
                cancellationToken,
                _idHandler.GenerateWithIds(_defaultWordsDatabase.Defaults));

            SetSortedWords();

            _wordsTimerService.Init(this);

            _disposable = _wordEntries
                .Subscribe(this, static (_, self) => self.UpdateCurrentWords());
        }

        private void UpdateCurrentWords()
        {
            foreach (var (practiceState, learningStates) in _appConfig.TargetStatesForCurrentWord.AsTuples())
            {
                foreach (var learningState in learningStates)
                {
                    var currentWordsByState = CurrentWordsByState.Value;
                    var minWord = SortedWordsByState.Value[learningState].Min;

                    if (minWord is null)
                        return;

                    currentWordsByState[practiceState] = minWord.Cooldown <= DateTime.Now ? minWord : null;
                    CurrentWordsByState.Value = currentWordsByState;
                }
            }
        }

        private void SetSortedWords()
        {
            foreach (var word in _wordEntries.Value.Values)
                SortedWordsByState.Value[word.LearningState].Add(word);
        }

        public List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count) =>
            _wordEntries.Value.Values.AsValueEnumerable()
                .Where(word => word != wordToSkip && word.IsHidden is false)
                .OrderBy(_ => Random.value)
                .Take(count)
                .ToList();

        public void Dispose()
        {
            _wordEntries?.Dispose();
            _disposable?.Dispose();
        }
    }
}