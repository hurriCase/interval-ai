using CustomUtils.Runtime.Encryption;
using CustomUtils.Runtime.Extensions;
using FirebaseServices.Runtime.Config;
using UnityEngine;

namespace Source.Scripts.Core.Api.Base
{
    internal abstract class ApiConfigBase : ScriptableObject
    {
        [field: SerializeField] internal float UpdateAvailabilityInterval { get; private set; }
        [field: SerializeField] internal string AvailabilityCheckUrl { get; private set; }
        [field: SerializeField] internal long AvailabilityCode { get; private set; }

        [SerializeField] protected string endpointFormat;

        [SerializeField] private string _encryptionKeyEnvironmentName;
        [SerializeField] private string _apiKeyEnvironmentName;

        private string _encryptedApiKey;
        private string _password;

        internal void Init(IRemoteConfigService remoteConfigService)
        {
            if (Application.isEditor)
                return;

            remoteConfigService.TryGetString(_encryptionKeyEnvironmentName, out _encryptedApiKey);
        }

        internal abstract string GetApiUrl();

        internal virtual bool IsValid() => string.IsNullOrEmpty(endpointFormat) is false && IsEncryptionValid();

        internal void SetPassword(string password) => _password = password;

        protected string GetApiKey()
        {
            if (Application.isEditor is false)
                return XORDataEncryption.Decrypt(_encryptedApiKey, _password);

            _apiKeyEnvironmentName.TryGetValueFromEnvironment(out var apiKey);
            return apiKey;
        }

        private bool IsEncryptionValid()
        {
            if (Application.isEditor)
                return true;

            return string.IsNullOrEmpty(_password) is false && string.IsNullOrEmpty(_encryptedApiKey) is false;
        }
    }
}