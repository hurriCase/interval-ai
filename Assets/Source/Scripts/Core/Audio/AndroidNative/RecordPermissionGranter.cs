using System;
using UnityEngine.Android;

namespace Source.Scripts.Core.Audio.AndroidNative
{
    internal struct RecordPermissionGranter
    {
        private const string MicrophonePermission = "android.permission.RECORD_AUDIO";

        internal static void RequestPermission<TSource>(TSource source, Action<TSource, bool> callback)
        {
            if (HasPermission())
            {
                callback?.Invoke(source, true);
                return;
            }

            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += _ => callback?.Invoke(source, true);
            callbacks.PermissionDenied += _ => callback?.Invoke(source, false);

            Permission.RequestUserPermission(MicrophonePermission, callbacks);
        }

        internal static bool HasPermission() => Permission.HasUserAuthorizedPermission(MicrophonePermission);
    }
}