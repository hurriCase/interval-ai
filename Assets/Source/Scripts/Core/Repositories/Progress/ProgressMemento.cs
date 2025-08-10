using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    internal partial class ProgressRepository
    {
        internal readonly struct ProgressMemento
        {
            private readonly int _currentStreak;
            private readonly int _bestStreak;
            private readonly EnumArray<LearningState, int> _totalCountByState;
            private readonly int _newWordsDailyTarget;
            private readonly Dictionary<DateTime, DailyProgress> _progressHistory;

            private readonly ProgressRepository _progressRepository;

            internal ProgressMemento(ProgressRepository progressRepository)
            {
                _currentStreak = progressRepository.CurrentStreak.CurrentValue;
                _bestStreak = progressRepository.BestStreak.CurrentValue;
                _totalCountByState = progressRepository.TotalCountByState.CurrentValue;
                _newWordsDailyTarget = progressRepository.NewWordsDailyTarget.CurrentValue;
                _progressHistory = new Dictionary<DateTime, DailyProgress>(progressRepository.ProgressHistory.CurrentValue);

                _progressRepository = progressRepository;
            }

            internal void Undo()
            {
                _progressRepository._currentStreak.Value = _currentStreak;
                _progressRepository._bestStreak.Value = _bestStreak;
                _progressRepository._totalCountByState.Value = _totalCountByState;
                _progressRepository._newWordsDailyTarget.Value = _newWordsDailyTarget;
                _progressRepository._progressHistory.Value = new Dictionary<DateTime, DailyProgress>(_progressHistory);
            }
        }
    }
}