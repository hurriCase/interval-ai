using System.Collections.Generic;
using Client.Scripts.Core;
using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader
{
    internal sealed class AutoPersistent : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void MakePersistent()
        {
            var dontDestroyObjects = DontDestroyLoader.LoadAllDontDestroy();
            var instantiatedObjects = new HashSet<string>();

            foreach (var dontDestroy in dontDestroyObjects)
            {
                if (instantiatedObjects.Contains(dontDestroy.name))
                    continue;

                if (dontDestroy.TryGetComponent<DontDestroyOnLoadComponent>(out _) is false)
                    continue;

                var instance = Instantiate(dontDestroy);
                instance.name = dontDestroy.name;
                DontDestroyOnLoad(instance);
                instantiatedObjects.Add(dontDestroy.name);
            }
        }
    }
}