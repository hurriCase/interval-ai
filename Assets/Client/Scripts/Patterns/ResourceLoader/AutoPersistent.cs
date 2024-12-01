using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader
{
    internal class AutoPersistent : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void MakePersistent()
        {
            var components = ResourceLoader.LoadAllDontDestroy<DontDestroyOnLoadComponent>();
            
            foreach (var component in components)
            {
                var instance = Instantiate(component);
                instance.name = component.name;
                DontDestroyOnLoad(instance);
            }
        }
    }
}