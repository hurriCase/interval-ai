using UnityEngine;

namespace Client.Scripts.Patterns
{
    internal abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        internal static T Instance { get; private set; }

        protected virtual bool IsPersistent { get; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;

            if (IsPersistent)
                DontDestroyOnLoad(this);

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}