using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Data.Repositories
{
    internal interface IRepository : IDisposable
    {
        UniTask InitAsync(CancellationToken cancellationToken);
    }
}