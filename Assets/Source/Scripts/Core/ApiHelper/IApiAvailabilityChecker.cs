using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.ApiHelper
{
    internal interface IApiAvailabilityChecker
    {
        UniTask<bool> IsAvailable(string url, long contentCode, CancellationToken token);
    }
}