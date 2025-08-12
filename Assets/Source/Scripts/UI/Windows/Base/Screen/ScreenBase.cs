using CustomUtils.Runtime.Extensions;

namespace Source.Scripts.UI.Windows.Base.Screen
{
    internal abstract class ScreenBase : WindowBase
    {
        internal override void Show()
        {
            CanvasGroup.Show();
        }

        internal override void Hide()
        {
            CanvasGroup.Hide();
        }
    }
}