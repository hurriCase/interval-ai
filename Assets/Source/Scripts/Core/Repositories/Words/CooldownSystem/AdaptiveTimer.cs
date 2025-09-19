using System;
using R3;

namespace Source.Scripts.Core.Repositories.Words.CooldownSystem
{
    internal struct AdaptiveTimer : IDisposable
    {
        public readonly Observable<DateTime> OnTimeUpdated => _timeUpdated;
        private readonly Subject<DateTime> _timeUpdated;

        private DateTime _targetTime;
        private IDisposable _currentTimer;

        internal AdaptiveTimer(DateTime targetTime)
        {
            _targetTime = targetTime;
            _timeUpdated = new Subject<DateTime>();
            _currentTimer = null;

            ScheduleNextUpdate();
        }

        internal void UpdateTargetTime(DateTime newTargetTime)
        {
            if (_targetTime == newTargetTime)
                return;

            _targetTime = newTargetTime;

            _currentTimer?.Dispose();
            ScheduleNextUpdate();
        }

        private void ScheduleNextUpdate()
        {
            var now = DateTime.Now;
            var timeRemaining = _targetTime - now;

            _timeUpdated.OnNext(now);

            if (timeRemaining.TotalSeconds <= 0)
            {
                _timeUpdated.OnCompleted();
                return;
            }

            var updateInterval = GetUpdateInterval(timeRemaining);

            _currentTimer = Observable.Timer(updateInterval)
                .Subscribe(this, (_, timer) => timer.ScheduleNextUpdate());
        }

        private static TimeSpan GetUpdateInterval(TimeSpan timeRemaining)
        {
            if (timeRemaining.TotalSeconds <= 60)
                return TimeSpan.FromSeconds(1);

            if (timeRemaining.TotalMinutes <= 60)
                return TimeSpan.FromMinutes(1);

            return timeRemaining.TotalHours <= 24 ? TimeSpan.FromHours(1) : TimeSpan.FromDays(1);
        }

        public readonly void Dispose()
        {
            _currentTimer?.Dispose();
            _timeUpdated?.Dispose();
        }
    }
}