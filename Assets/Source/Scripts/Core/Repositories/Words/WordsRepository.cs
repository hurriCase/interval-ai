using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;
using Source.Scripts.Core.Repositories.Words.Timer;
using ZLinq;
using Random = UnityEngine.Random;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class WordsRepository : IWordsRepository, IRepository
    {
        //TODO:<Dmitriy.Sukharev> make this immutable
        public PersistentReactiveProperty<Dictionary<int, WordEntry>> WordEntries { get; } = new();
        public ReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> SortedWordsByState { get; }
            = new(new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer)));
        public ReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState { get; }
            = new(new EnumArray<PracticeState, WordEntry>(EnumMode.SkipFirst));

        private static readonly WordCooldownComparer _comparer = new();

        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IIdHandler<WordEntry> _idHandler;
        private readonly IDefaultSettingsConfig _defaultSettingsConfig;

        private IDisposable _disposable;

        internal WordsRepository(
            DefaultWordsDatabase defaultWordsDatabase,
            IWordsTimerService wordsTimerService,
            IIdHandler<WordEntry> idHandler,
            IDefaultSettingsConfig defaultSettingsConfig)
        {
            _defaultWordsDatabase = defaultWordsDatabase;
            _wordsTimerService = wordsTimerService;
            _idHandler = idHandler;
            _defaultSettingsConfig = defaultSettingsConfig;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                _idHandler.InitAsync(cancellationToken),

                WordEntries.InitAsync(
                    PersistentKeys.WordEntryKey,
                    cancellationToken,
                    _idHandler.GenerateWithIds(_defaultWordsDatabase.Defaults))
            };

            await UniTask.WhenAll(initTasks);

            await WordEntries.InitAsync(
                PersistentKeys.WordEntryKey,
                cancellationToken,
                _idHandler.GenerateWithIds(_defaultWordsDatabase.Defaults));

            SetSortedWords();

            _wordsTimerService.Init(this);

            _disposable = WordEntries
                .Subscribe(this, static (_, self) => self.UpdateCurrentWords());
        }

        private void UpdateCurrentWords()
        {
            foreach (var (practiceState, learningStates) in
                     _defaultSettingsConfig.PracticeToLearningStates.AsTuples())
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
            foreach (var word in WordEntries.Value.Values)
                SortedWordsByState.Value[word.LearningState].Add(word);
        }

        public List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count) =>
            WordEntries.Value.Values.AsValueEnumerable()
                .Where(word => word != wordToSkip && word.IsHidden is false)
                .OrderBy(_ => Random.value)
                .Take(count)
                .ToList();

        public void Dispose()
        {
            WordEntries?.Dispose();
            _disposable?.Dispose();
        }
    }
}