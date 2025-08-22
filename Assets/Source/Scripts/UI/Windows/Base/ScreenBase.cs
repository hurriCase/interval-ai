using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class ScreenBase : WindowBase
    {
        internal override UniTask ShowAsync()
        {
            CanvasGroup.Show();
            return UniTask.CompletedTask;
        }

        internal override UniTask HideAsync()
        {
            CanvasGroup.Hide();
            return UniTask.CompletedTask;
        }
    }
}