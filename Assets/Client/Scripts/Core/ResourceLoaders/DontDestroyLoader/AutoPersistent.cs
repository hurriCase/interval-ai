using Client.Scripts.Core;
using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader
{
    internal sealed class AutoPersistent : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void MakePersistent()
        {
            var components = DontDestroyLoader.LoadAllDontDestroy();

            foreach (var component in components)
            {
                if (component.TryGetComponent<DontDestroyOnLoadComponent>(out var dontDestroyComponent) is false ||
                    dontDestroyComponent.IsInstantiated)
                    continue;

                var instance = Instantiate(component);
                instance.name = component.name;
                DontDestroyOnLoad(instance);
                dontDestroyComponent.IsInstantiated = true;
            }
        }
    }
}