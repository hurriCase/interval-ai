using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
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
        public ReadOnlyReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> SortedWordsByState
            => _sortedWordsByState;

        private readonly ReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> _sortedWordsByState
            = new(new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer)));

        private readonly PersistentReactiveProperty<Dictionary<int, WordEntry>> _wordEntries = new();
        private static readonly WordCooldownComparer _comparer = new();

        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IIdHandler<WordEntry> _idHandler;

        private DisposableBag _disposable;

        internal WordsRepository(DefaultWordsDatabase defaultWordsDatabase, IIdHandler<WordEntry> idHandler)
        {
            _defaultWordsDatabase = defaultWordsDatabase;
            _idHandler = idHandler;
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
        }

        public void RemoveHiddenWord(WordEntry word)
        {
            if (word.IsHidden is false)
                return;

            _sortedWordsByState.Value[word.LearningState].Remove(word);
            _sortedWordsByState.OnNext(_sortedWordsByState.Value);
        }

        public void AddWord(TranslationSet translationSet)
        {
            var newWord = new WordEntry { Word = translationSet };

            _wordEntries.Value.Add(_idHandler.GetId(), newWord);

            _sortedWordsByState.Value[newWord.LearningState].Add(newWord);
            _sortedWordsByState.OnNext(_sortedWordsByState.Value);
        }

        public void OnWordStateChanged(WordEntry word, LearningState oldState, LearningState newState)
        {
            _sortedWordsByState.Value[oldState].Remove(word);
            _sortedWordsByState.Value[newState].Add(word);

            _sortedWordsByState.OnNext(_sortedWordsByState.Value);
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
            _wordEntries.Dispose();
            _disposable.Dispose();
        }
    }
}