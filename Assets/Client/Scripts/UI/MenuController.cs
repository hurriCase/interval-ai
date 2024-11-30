using Client.Scripts.UI.Base;
using Client.Scripts.UI.MainWindows;
using UnityEngine;

namespace Client.Scripts.UI
{
    internal sealed class MenuController : MonoBehaviour
    {
        public void OpenCategoriesWindow() => WindowsController.Instance.OpenWindow<CategoriesWindow>();
        public void OpenLearnWordsWindow() => WindowsController.Instance.OpenWindow<LearnWordsWindow>();
        public void OpenSentencesWindow() => WindowsController.Instance.OpenWindow<SentencesWindow>();
        public void OpenSettingsWindow() => WindowsController.Instance.OpenWindow<SettingsWindow>();
    }
}