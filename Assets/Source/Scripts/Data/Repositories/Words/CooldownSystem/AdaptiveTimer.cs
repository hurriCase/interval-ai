using System;
using R3;

namespace Source.Scripts.Data.Repositories.Categories.CooldownSystem
{
    internal struct AdaptiveTimer : IDisposable
    {
        private DateTime _targetTime;
        private readonly Subject<DateTime> _timeSubject;
        private IDisposable _currentTimer;

        public readonly Observable<DateTime> TimeUpdates => _timeSubject.AsObservable();

        internal AdaptiveTimer(DateTime targetTime)
        {
            _targetTime = targetTime;
            _timeSubject = new Subject<DateTime>();
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

            _timeSubject.OnNext(now);

            if (timeRemaining.TotalSeconds <= 0)
            {
                _timeSubject.OnCompleted();
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
            _timeSubject?.Dispose();
        }
    }
}