using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Repositories
{
    internal interface IDefaultConfig
    {
        UniTask InitAsync(CancellationToken cancellation);
    }
}