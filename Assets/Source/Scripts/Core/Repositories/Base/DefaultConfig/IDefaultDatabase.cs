using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Repositories.Base.DefaultConfig
{
    internal interface IDefaultDatabase
    {
        UniTask InitAsync(CancellationToken cancellation);
    }
}