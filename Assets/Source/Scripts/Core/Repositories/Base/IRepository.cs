using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Repositories.Base
{
    internal interface IRepository : IDisposable
    {
        UniTask InitAsync(CancellationToken cancellationToken);
    }
}