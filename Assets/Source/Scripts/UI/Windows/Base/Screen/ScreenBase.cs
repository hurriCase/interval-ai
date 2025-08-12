using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base.Screen
{
    internal abstract class ScreenBase : WindowBase<ScreenType>
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