using System;
using UnityEngine;

namespace Client.Scripts.Patterns
{
    internal abstract class ObservableScriptableObject<T> : ScriptableObject where T : class
    {
        public event Action<T> OnValueChanged;

        protected void NotifyValueChanged(T value) 
            => OnValueChanged?.Invoke(value);
    }
}