using System.Diagnostics;
using CustomUtils.Runtime.AddressableSystem;
using Debug = UnityEngine.Debug;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    internal static class AndroidLogger
    {
        [Conditional("ANDROID_LOG_ALL")]
        internal static void Log(string message)
        {
            Debug.Log(message);
        }

        [Conditional("ANDROID_LOG_ALL")]
        internal static void LogWarning(string message)
        {
            Debug.Log(message);
        }

        [Conditional("ANDROID_LOG_ALL")]
        internal static void LogError(string message)
        {
            Debug.Log(message);
        }

        internal static StopWatchScope LogWithTimePast(string message) => new(message);
    }
}