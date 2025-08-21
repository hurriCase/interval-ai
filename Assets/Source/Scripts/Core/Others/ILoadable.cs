using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Others
{
    internal interface ILoadable
    {
        UniTask InitAsync(CancellationToken cancellationToken);
    }
}