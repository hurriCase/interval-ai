using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Source.Scripts.Core.ApiHelper
{
    internal interface IApiService
    {
        ReadOnlyReactiveProperty<bool> IsAvailable { get; }
        UniTask UpdateAvailable(CancellationToken token);
    }
}