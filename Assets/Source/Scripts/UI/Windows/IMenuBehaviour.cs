using System.Threading;

namespace Source.Scripts.UI.Windows
{
    internal interface IMenuBehaviour
    {
        void Init(CancellationToken cancellationToken);
    }
}