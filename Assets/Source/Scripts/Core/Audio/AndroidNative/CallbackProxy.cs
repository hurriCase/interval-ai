using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Audio.AndroidNative
{
    internal sealed class CallbackProxy<T> : AndroidJavaProxy
    {
        private readonly Action<T> _callback;

        internal CallbackProxy(Action<T> callback, string javaInterface) : base(javaInterface)
        {
            _callback = callback;
        }

        // ReSharper disable once InconsistentNaming || Kotlin side method signature
        [Preserve]
        public void onResult(T value)
        {
            // Switch back to the Unity main thread
            UniTask.Post(() =>
            {
                _callback?.Invoke(value);
            });
        }
    }
}