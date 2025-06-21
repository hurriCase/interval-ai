using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class ScreenBase : WindowBase<ScreenType>
    {
        [field: SerializeField] internal bool InitialWindow { get; private set; }
    }
}