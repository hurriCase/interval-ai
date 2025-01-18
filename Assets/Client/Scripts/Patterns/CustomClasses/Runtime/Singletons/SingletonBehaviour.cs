using DependencyInjection.Runtime;
using UnityEngine;

namespace CustomClasses.Runtime.Singletons
{
    /// <summary>
    /// Base class for MonoBehaviours that follow the Singleton pattern.
    /// Ensures only one instance exists and automatically handles cleanup.
    /// </summary>
    /// <typeparam name="T">The type of MonoBehaviour to make a singleton.</typeparam>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance)
                Destroy(gameObject);

            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance != this)
                return;

            Instance = null;
            DIContainer.ClearSingletonDependency<T>();
        }
    }
}