using Client.Scripts.UI.Base;
using Client.Scripts.UI.MainWindows;
using UnityEngine;

namespace Client.Scripts.UI
{
    internal sealed class MainSceneController : MonoBehaviour
    {
        private void Start()
        {
            WindowsController.Instance.Init();
            WindowsController.Instance.OpenWindow<LearnWordsWindow>();
        }
    }
}