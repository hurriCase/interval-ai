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
using Source.Scripts.Core.Repositories.Words.Word;
using ZLinq;
using Random = UnityEngine.Random;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class WordsRepository : IWordsRepository, IRepository, IDisposable
    {
        private readonly ReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> _sortedWordsByState
            = new(new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer)));

        public ReadOnlyReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState =>
            _currentWordsByState;

        private readonly ReactiveProperty<EnumArray<PracticeState, WordEntry>> _currentWordsByState
            = new(new EnumArray<PracticeState, WordEntry>(EnumMode.SkipFirst));

        private readonly PersistentReactiveProperty<Dictionary<int, WordEntry>> _wordEntries = new();
        private static readonly WordCooldownComparer _comparer = new();

        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IIdHandler<WordEntry> _idHandler;
        private readonly IAppConfig _appConfig;

        private DisposableBag _disposable;

        internal WordsRepository(
            DefaultWordsDatabase defaultWordsDatabase,
            IIdHandler<WordEntry> idHandler,
            IAppConfig appConfig)
        {
            _defaultWordsDatabase = defaultWordsDatabase;
            _idHandler = idHandler;
            _appConfig = appConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                _idHandler.InitAsync(token),

                _wordEntries.InitAsync(
                    PersistentKeys.WordEntryKey,
                    token,
                    _idHandler.GenerateWithIds(_defaultWordsDatabase.Defaults))
            };

            await UniTask.WhenAll(initTasks);

            SetSortedWords();

            _wordEntries
                .Subscribe(this, static (_, self) => self.UpdateCurrentWords())
                .AddTo(ref _disposable);
        }

        public void SetCurrentWord(PracticeState practiceState, WordEntry word)
        {
            var currentWordsByState = _currentWordsByState.Value;
            currentWordsByState[practiceState] = word;
            _currentWordsByState.Value = currentWordsByState;
        }

        public void RemoveHiddenWord(WordEntry word)
        {
            if (word.IsHidden is false)
                return;

            _sortedWordsByState.Value[word.LearningState].Remove(word);
            UpdateCurrentWords();
        }

        public void UpdateCurrentWords()
        {
            foreach (var (practiceState, learningStates) in
                     _appConfig.TargetLearningStatesForPractice.AsTuples())
            {
                foreach (var learningState in learningStates)
                {
                    var currentWordsByState = _currentWordsByState.Value;
                    currentWordsByState[practiceState] = _sortedWordsByState.Value[learningState].Min;
                    _currentWordsByState.Value = currentWordsByState;
                    _currentWordsByState.OnNext(currentWordsByState);

                    if (currentWordsByState[practiceState] != null)
                        break;
                }
            }

            _wordEntries.SaveAsync();
        }

        private void SetSortedWords()
        {
            foreach (var word in _wordEntries.Value.Values)
            {
                if (word.IsHidden)
                    continue;

                _sortedWordsByState.Value[word.LearningState].Add(word);
            }
        }

        public List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count) =>
            _wordEntries.Value.Values.AsValueEnumerable()
                .Where(word => word != wordToSkip && word.IsHidden is false)
                .OrderBy(_ => Random.value)
                .Take(count)
                .ToList();

        public void Dispose()
        {
            _sortedWordsByState.Dispose();
            _currentWordsByState.Dispose();
            _wordEntries.Dispose();
            _disposable.Dispose();
        }
    }
}