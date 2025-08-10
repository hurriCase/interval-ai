using System;
using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Timer;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal sealed class WordAdvanceService : IWordAdvanceService, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> CanUndo => _canUndo;
        public ReactiveCommand UndoCommand { get; } = new();

        private readonly IWordsRepository _wordsRepository;
        private readonly IWordStateMutator _wordStateMutator;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IProgressRepository _progressRepository;

        private readonly Stack<(WordMemento, ProgressRepository.ProgressMemento)> _undoStack = new();
        private readonly ReactiveProperty<bool> _canUndo = new(false);

        internal WordAdvanceService(
            IWordsRepository wordsRepository,
            IWordStateMutator wordStateMutator,
            IWordsTimerService wordsTimerService,
            IProgressRepository progressRepository)
        {
            _wordsRepository = wordsRepository;
            _wordStateMutator = wordStateMutator;
            _wordsTimerService = wordsTimerService;
            _progressRepository = progressRepository;

            UndoCommand.Subscribe(this, static (_, service) => service.ExecuteUndo());
        }

        public void AdvanceWord(WordEntry word, bool success)
        {
            UpdateUndo(word);

            _wordStateMutator.AdvanceLearningState(word, success);
            _wordsTimerService.UpdateTimers();
            _wordsRepository.UpdateCurrentWords();
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

        public void Dispose()
        {
            _canUndo.Dispose();
            UndoCommand.Dispose();
        }
    }
}