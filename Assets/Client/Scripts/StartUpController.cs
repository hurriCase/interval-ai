using Client.Scripts.Database;
using Client.Scripts.UI.Base;
using Client.Scripts.UI.MainWindows;
using Unity.VisualScripting;
using UnityEngine;

namespace Client.Scripts
{
    internal sealed class StartUpController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            AudioController.Instance.PlayMusic();
            DBController.Instance.Init();
        }
    }
}