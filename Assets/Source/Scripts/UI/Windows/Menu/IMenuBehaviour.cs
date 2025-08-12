using System.Threading;

namespace Source.Scripts.UI.Windows.Menu
{
    internal interface IMenuBehaviour
    {
        void Init(CancellationToken cancellationToken);
    }
}