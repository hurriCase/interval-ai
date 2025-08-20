using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Repositories.Base
{
    internal interface IRepository
    {
        UniTask InitAsync(CancellationToken cancellationToken);
    }
}