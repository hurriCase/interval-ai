﻿using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Source.Scripts.Core.Other
{
    internal static class AddressablesLogger
    {
        [Conditional("ADDRESSABLES_LOG_ALL")]
        internal static void Log(string message)
        {
            Debug.Log(message);
        }

        [Conditional("ADDRESSABLES_LOG_ALL")]
        internal static void LogWarning(string message)
        {
            Debug.Log(message);
        }

        [Conditional("ADDRESSABLES_LOG_ALL")]
        internal static void LogError(string message)
        {
            Debug.Log(message);
        }

        internal static StopWatchScope LogWithTimePast(string message) => new(message);
    }
}