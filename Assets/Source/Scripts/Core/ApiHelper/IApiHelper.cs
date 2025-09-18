#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Source.Scripts.Core.ApiHelper
{
    internal interface IApiHelper
    {
        UniTask<TResponse?> PostAsync<TRequest, TResponse>(TRequest request, string url, CancellationToken token)
            where TRequest : class
            where TResponse : class;

        UniTask<TResponse?> PostAsync<TSource, TRequest, TResponse>(
            TSource source,
            TRequest request,
            string url,
            CancellationToken token,
            Action<TSource, UnityWebRequest> additionalHeaders)
            where TRequest : class
            where TResponse : class;
    }
}