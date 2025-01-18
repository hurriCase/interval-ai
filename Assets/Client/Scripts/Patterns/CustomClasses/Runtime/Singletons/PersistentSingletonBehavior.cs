using AssetLoader.Runtime;
using DependencyInjection.Runtime;
using UnityEngine;

// ReSharper disable StaticMemberInGenericType
namespace CustomClasses.Runtime.Singletons
{
    /// <summary>
    /// Base class for MonoBehaviours that follow the Singleton pattern and persist between scenes.
    /// Can be instantiated from a prefab in Resources folder or created dynamically.
    /// </summary>
    /// <typeparam name="T">The type of MonoBehaviour to make a persistent singleton.</typeparam>
    public abstract class PersistentSingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _isCreated;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_isCreated is false)
                    CreateInstance();

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance != this)
                return;

            _instance = null;
            _isCreated = false;
            DIContainer.ClearSingletonDependency<T>();
        }

        private static void CreateInstance()
        {
            if (_isCreated)
                return;

            var type = typeof(T);
            var prefabName = type.Name;

            if (ResourceLoader<T>.TryLoad(out var prefab))
                prefabName = prefab.name;

            var gameObject = prefab ? Instantiate(prefab.gameObject) : new GameObject(prefabName);

            _instance ??= gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();

            _isCreated = true;
        }
    }
}