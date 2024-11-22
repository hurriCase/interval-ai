using UnityEngine;

namespace Client.Scripts
{
    internal sealed class StartUpController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            AudioController.Instance.PlayMusic();
        }
    }
}