using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal readonly struct ProgressMemento
    {
        private readonly int _currentStreak;
        private readonly int _bestStreak;
        private readonly EnumArray<LearningState, int> _totalCountByState;
        private readonly int _newWordsDailyTarget;
        private readonly Dictionary<DateTime, DailyProgress> _progressHistory;

        private readonly IProgressRepository _progressRepository;

        internal ProgressMemento(IProgressRepository progressRepository)
        {
            _currentStreak = progressRepository.CurrentStreak.Value;
            _bestStreak = progressRepository.BestStreak.Value;
            _totalCountByState = progressRepository.TotalCountByState.Value;
            _newWordsDailyTarget = progressRepository.NewWordsDailyTarget.Value;
            _progressHistory = new Dictionary<DateTime, DailyProgress>(progressRepository.ProgressHistory.Value);

            _progressRepository = progressRepository;
        }

        internal void Undo()
        {
            _progressRepository.CurrentStreak.Value = _currentStreak;
            _progressRepository.BestStreak.Value = _bestStreak;
            _progressRepository.TotalCountByState.Value = _totalCountByState;
            _progressRepository.NewWordsDailyTarget.Value = _newWordsDailyTarget;
            _progressRepository.ProgressHistory.Value = new Dictionary<DateTime, DailyProgress>(_progressHistory);
        }
    }
}