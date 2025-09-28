using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Api.Interfaces
{
    internal interface IApiAvailabilityChecker
    {
        UniTask<bool> IsAvailable(string url, long contentCode, CancellationToken token);
    }
}