using System;
using UnityEngine;

// ReSharper disable StaticMemberInGenericType
namespace Client.Scripts.Patterns
{
    internal abstract class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : MonoBehaviour
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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

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