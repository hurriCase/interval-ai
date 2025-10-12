using System;
using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Core.Repositories.Words.Word;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    [Preserve]
    internal sealed class WordAdvanceService : IWordAdvanceService, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> CanUndo => _canUndo;
        public ReactiveCommand UndoCommand { get; } = new();

        private readonly Stack<(WordMemento, ProgressRepository.ProgressMemento)> _undoStack = new();
        private readonly ReactiveProperty<bool> _canUndo = new(false);

        private readonly ICurrentWordService _currentWordService;
        private readonly IProgressRepository _progressRepository;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IWordStateMutator _wordStateMutator;

        [Preserve]
        internal WordAdvanceService(
            PracticeState practiceState,
            ICurrentWordFactory currentWordFactory,
            IProgressRepository progressRepository,
            IWordsTimerService wordsTimerService,
            IWordStateMutator wordStateMutator)
        {
            _progressRepository = progressRepository;
            _wordsTimerService = wordsTimerService;
            _wordStateMutator = wordStateMutator;

            _currentWordService = currentWordFactory.GetOrCreate(practiceState);

            UndoCommand.Subscribe(this, static (_, self) => self.ExecuteUndo());
        }

        public void AdvanceWord(WordEntry word, bool success)
        {
            UpdateUndo(word);

            _wordStateMutator.AdvanceLearningState(word, success);
            _wordsTimerService.UpdateTimer();
            _currentWordService.UpdateCurrentWord();
        }

        private void ExecuteUndo()
        {
            if (_undoStack.Count == 0)
                return;

            var (wordMemento, progressMemento) = _undoStack.Pop();

            wordMemento.Undo();
            progressMemento.Undo();

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