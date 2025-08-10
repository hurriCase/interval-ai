using System;
using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Timer;
using ZLinq;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal sealed class WordAdvanceService : IWordAdvanceService, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> CanUndo => _canUndo;
        public ReactiveCommand UndoCommand { get; } = new();

        private readonly IWordsRepository _wordsRepository;
        private readonly IAppConfig _appConfig;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IProgressRepository _progressRepository;
        private readonly ISettingsRepository _settingsRepository;

        private readonly Stack<(WordMemento, ProgressRepository.ProgressMemento)> _undoStack = new();
        private readonly ReactiveProperty<bool> _canUndo = new(false);

        internal WordAdvanceService(
            IWordsRepository wordsRepository,
            IAppConfig appConfig,
            IWordsTimerService wordsTimerService,
            IProgressRepository progressRepository,
            ISettingsRepository settingsRepository)
        {
            _wordsRepository = wordsRepository;
            _appConfig = appConfig;
            _wordsTimerService = wordsTimerService;
            _progressRepository = progressRepository;
            _settingsRepository = settingsRepository;

            UndoCommand.Subscribe(this, static (_, service) => service.ExecuteUndo());
        }

        public void AdvanceWord(WordEntry word, bool success)
        {
            UpdateUndo(word);

            var sortedWords = _wordsRepository.SortedWordsByState;
            sortedWords.Value[word.LearningState].Remove(word);

            word.AdvanceLearningState(success);

            if (_appConfig.CooldownStates.AsValueEnumerable().Contains(word.LearningState))
                AdvanceCooldown(word);

            sortedWords.Value[word.LearningState].Add(word);
            sortedWords.OnNext(sortedWords.Value);

            _wordsRepository.WordEntries.SaveAsync();
        }

        private void ExecuteUndo()
        {
            if (_undoStack.Count == 0)
                return;

            var (wordState, progressState) = _undoStack.Pop();

            wordState.Undo();
            progressState.Undo();

            _canUndo.Value = _undoStack.Count > 0;
        }

        private void UpdateUndo(WordEntry word)
        {
            var wordState = new WordMemento(word);
            var progressState = _progressRepository.CreateMemento();

            _undoStack.Push((wordState, progressState));
            _canUndo.Value = _undoStack.Count > 0;
        }

        private void AdvanceCooldown(WordEntry word)
        {
            var oldState = word.LearningState;

            var cooldownData = _settingsRepository.RepetitionByCooldown.Value[word.RepetitionCount];
            word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);

            _wordsTimerService.UpdateTimerForState(oldState);
            _wordsTimerService.UpdateTimerForState(word.LearningState);

            _wordsRepository.WordEntries.SaveAsync();
        }

        public void Dispose()
        {
            _canUndo.Dispose();
            UndoCommand.Dispose();
        }
    }
}