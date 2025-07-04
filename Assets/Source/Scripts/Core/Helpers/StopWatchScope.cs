using System;
using System.Diagnostics;

namespace Source.Scripts.Core.Helpers
{
    internal readonly struct StopWatchScope : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _message;

        internal StopWatchScope(string message)
        {
            _stopwatch = Stopwatch.StartNew();
            _message = message;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            AddressablesLogger.Log($"{_message} Operation was done in {_stopwatch.ElapsedMilliseconds}ms");
        }
    }
}