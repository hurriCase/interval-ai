using System;
using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Timer;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal sealed class WordAdvanceService : IWordAdvanceService
    {
        public Observable<bool> CanUndo => _canUndo.AsObservable();

        private readonly IWordsRepository _wordsRepository;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IProgressRepository _progressRepository;
        private readonly ISettingsRepository _settingsRepository;

        private readonly Stack<(WordMemento, ProgressMemento)> _undoStack = new();
        private readonly Subject<bool> _canUndo = new();

        internal WordAdvanceService(
            IWordsRepository wordsRepository,
            IWordsTimerService wordsTimerService,
            IProgressRepository progressRepository,
            ISettingsRepository settingsRepository)
        {
            _wordsRepository = wordsRepository;
            _wordsTimerService = wordsTimerService;
            _progressRepository = progressRepository;
            _settingsRepository = settingsRepository;
        }

        public void AdvanceWord(WordEntry word, bool success)
        {
            var wordState = new WordMemento(word);
            var progressState = new ProgressMemento(_progressRepository);

            if (success)
                HandleSuccess(word);
            else
                HandleFailure(word);

            _wordsRepository.WordEntries.SaveAsync();
            _undoStack.Push((wordState, progressState));
            _canUndo.OnNext(_undoStack.Count > 0);
        }

        public void UndoWordAdvance()
        {
            if (_undoStack.Count == 0)
                return;

            var (wordState, progressState) = _undoStack.Pop();

            wordState.Undo();
            progressState.Undo();

            _canUndo.OnNext(_undoStack.Count > 0);
        }

        private void HandleSuccess(WordEntry word)
        {
            _wordsRepository.SortedWordsByState.Value[word.LearningState].Remove(word);

            switch (word.LearningState)
            {
                case LearningState.None:
                    word.LearningState = LearningState.CurrentlyLearning;
                    break;

                case LearningState.CurrentlyLearning:
                    HandleSuccessCurrentlyLearning(word);
                    break;

                case LearningState.Repeatable:
                    HandleSuccessRepeatable(word);
                    break;

                case LearningState.AlreadyKnown:
                case LearningState.Studied:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _wordsRepository.SortedWordsByState.Value[word.LearningState].Add(word);
        }

        private void HandleSuccessCurrentlyLearning(WordEntry word)
        {
            _progressRepository.IncrementLearnedWordsCount(PracticeState.NewWords);
            _progressRepository.NewWordsDailyTarget.Value--;

            word.LearningState = LearningState.Repeatable;

            AdvanceCooldown(word);
        }

        private void HandleSuccessRepeatable(WordEntry word)
        {
            _progressRepository.IncrementLearnedWordsCount(PracticeState.Review);

            if (word.RepetitionCount >= _settingsRepository.RepetitionByCooldown.Value.Count)
            {
                word.LearningState = LearningState.Studied;
                return;
            }

            word.RepetitionCount++;

            AdvanceCooldown(word);
        }

        private void HandleFailure(WordEntry word)
        {
            _wordsRepository.SortedWordsByState.Value[word.LearningState].Remove(word);

            switch (word.LearningState)
            {
                case LearningState.None:
                    word.LearningState = LearningState.AlreadyKnown;
                    break;

                case LearningState.Repeatable:
                    word.RepetitionCount = Math.Max(0, word.RepetitionCount - 1);
                    AdvanceCooldown(word);
                    break;

                case LearningState.AlreadyKnown:
                case LearningState.CurrentlyLearning:
                case LearningState.Studied:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _wordsRepository.SortedWordsByState.Value[word.LearningState].Add(word);
        }

        private void AdvanceCooldown(WordEntry word)
        {
            var oldState = word.LearningState;

            if (word.RepetitionCount >= _settingsRepository.RepetitionByCooldown.Value.Count)
                return;

            var cooldownData = _settingsRepository.RepetitionByCooldown.Value[word.RepetitionCount];
            word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);

            _wordsTimerService.UpdateTimerForState(oldState);
            _wordsTimerService.UpdateTimerForState(word.LearningState);

            _wordsRepository.WordEntries.SaveAsync();
        }
    }
}