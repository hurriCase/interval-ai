using CustomUtils.Runtime.Extensions;
using FirebaseServices.Runtime.Config;
using UnityEngine;

namespace Source.Scripts.Core.ApiHelper
{
    internal class ApiConfigBase : ScriptableObject
    {
        [field: SerializeField] internal string ApiKeyName { get; private set; }

        internal string ApiKey { get; private set; }

        internal void Init(IRemoteConfigService remoteConfigService)
        {
#if UNITY_EDITOR
            if (ApiKeyName.TryGetValueFromEnvironment(out var apyKey))
#else
            if (remoteConfigService.TryGetString(out var apyKey))
#endif
                ApiKey = apyKey;
        }
    }
}