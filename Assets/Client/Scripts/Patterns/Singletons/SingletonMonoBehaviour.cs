using System;
using UnityEngine;

// ReSharper disable StaticMemberInGenericType
namespace Client.Scripts.Patterns.Singletons
{
    internal abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _isCreated;
        private static bool _isDontDestroyOnload;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (Attribute.IsDefined(typeof(T), typeof(ResourceAttribute)))
                    _isDontDestroyOnload = true;

                if (_isCreated is false && _isDontDestroyOnload)
                    CreateInstance();

                return _instance;
            }
        }

        private void Awake()
        {
            if (_isDontDestroyOnload)
                DontDestroyOnLoad(gameObject);
            else
            {
                if (Instance)
                {
                    Destroy(gameObject);
                    return;
                }

                _instance = this as T;
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }

        private static void CreateInstance()
        {
            if (_isCreated)
                return;

            GameObject gameObject;
            string prefabName;
            var type = typeof(T);

            if (Attribute.GetCustomAttribute(type, typeof(ResourceAttribute)) is ResourceAttribute attribute)
            {
                prefabName = attribute.Name;

                var prefabPath = attribute.Path;
                var prefab = Resources.Load<GameObject>(prefabPath);

                if (prefab == null)
                {
                    Debug.LogError(
                        $"Could not find Prefab '{prefabName}' on Resources for Singleton of type '{type}' on path '{prefabPath}'.");
                    return;
                }

                gameObject = Instantiate(prefab);
            }
            else
            {
                prefabName = type.Name;
                gameObject = new GameObject();
            }

            gameObject.name = prefabName;

            if (_instance is null)
                _instance = gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();

            _isCreated = true;
        }
    }
}