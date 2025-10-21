using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class ScreenBase : WindowBase
    {
        [field: SerializeField] internal bool InitialWindow { get; private set; }

        internal override UniTask ShowAsync()
        {
            canvasGroup.Show();
            return UniTask.CompletedTask;
        }

        internal override UniTask HideAsync()
        {
            canvasGroup.Hide();
            return UniTask.CompletedTask;
        }
    }
}