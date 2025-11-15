using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Audio.AndroidNative
{
    internal sealed class KotlinFunction1Callback : AndroidJavaProxy
    {
        private readonly Action<string> _callback;

        internal KotlinFunction1Callback(Action<string> callback) : base("kotlin.jvm.functions.Function1")
        {
            _callback = callback;
        }

        // ReSharper disable once InconsistentNaming || Kotlin side method signature
        [Preserve]
        public AndroidJavaObject invoke(AndroidJavaObject result)
        {
            var text = result.Call<string>("toString");
            _callback?.Invoke(text);
            return null;
        }
    }
}