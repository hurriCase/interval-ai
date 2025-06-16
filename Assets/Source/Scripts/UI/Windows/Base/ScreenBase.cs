using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal class ScreenBase : WindowBase<ScreenType>
    {
        [field: SerializeField] internal bool InitialWindow { get; private set; }
    }
}