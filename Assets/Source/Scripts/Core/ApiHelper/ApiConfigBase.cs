using CustomUtils.Runtime.Extensions;
using FirebaseServices.Runtime.Config;
using UnityEngine;

namespace Source.Scripts.Core.ApiHelper
{
    internal abstract class ApiConfigBase : ScriptableObject
    {
        [field: SerializeField] internal string ApiKeyName { get; private set; }

        [SerializeField] protected string endpointFormat;

        protected string apiKey;

        internal void Init(IRemoteConfigService remoteConfigService)
        {
            if (Application.isEditor)
            {
                ApiKeyName.TryGetValueFromEnvironment(out apiKey);
                return;
            }

            remoteConfigService.TryGetString(ApiKeyName, out apiKey);
        }

        internal abstract string GetApiUrl();

        internal virtual bool IsValidUrl() => string.IsNullOrEmpty(apiKey) is false &&
                                              string.IsNullOrEmpty(endpointFormat) is false;
    }
}