using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class ScreenBase : WindowBase<ScreenType>
    {
        [field: SerializeField] internal bool InitialWindow { get; private set; }

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