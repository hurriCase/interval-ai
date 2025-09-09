#nullable enable
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.ApiHelper
{
    internal interface IApiHelper
    {
        UniTask<TResponse?> PostAsync<TRequest, TResponse>(TRequest request, string url)
            where TRequest : class
            where TResponse : class;
    }
}