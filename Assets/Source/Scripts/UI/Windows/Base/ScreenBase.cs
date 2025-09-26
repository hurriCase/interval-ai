using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class ScreenBase : WindowBase
    {
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