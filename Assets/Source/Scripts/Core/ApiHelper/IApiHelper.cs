#nullable enable
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Source.Scripts.Core.ApiHelper
{
    internal interface IApiHelper
    {
        UniTask<TResponse?> PostAsync<TRequest, TResponse>(TRequest request, string url)
            where TRequest : class
            where TResponse : class;

        UniTask<TResponse?> PostAsync<TSource, TRequest, TResponse>(TSource source, TRequest request, string url,
            Action<TSource, UnityWebRequest> additionalHeaders)
            where TRequest : class
            where TResponse : class;
    }
}