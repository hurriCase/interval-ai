using CustomUtils.Runtime.Encryption;
using CustomUtils.Runtime.Extensions;
using FirebaseServices.Runtime.Config;
using UnityEngine;

namespace Source.Scripts.Core.ApiHelper
{
    internal abstract class ApiConfigBase : ScriptableObject
    {
        [field: SerializeField] internal string EncryptionKeyEnvironmentName { get; private set; }

        [SerializeField] protected string endpointFormat;

        private string _encryptedApiKey;
        private string _password;

        internal void Init(IRemoteConfigService remoteConfigService)
        {
            if (Application.isEditor)
                return;

            remoteConfigService.TryGetString(EncryptionKeyEnvironmentName, out _encryptedApiKey);
        }

        internal abstract string GetApiUrl();

        internal virtual bool IsValidUrl() => string.IsNullOrEmpty(endpointFormat) is false
                                              && (Application.isEditor
                                                  || (string.IsNullOrEmpty(_password) is false
                                                      && string.IsNullOrEmpty(_encryptedApiKey) is false));

        internal void SetPassword(string password) => _password = password;

        protected string GetApiKey()
        {
            if (Application.isEditor is false)
                return XORDataEncryption.Decrypt(_encryptedApiKey, _password);

            EncryptionKeyEnvironmentName.TryGetValueFromEnvironment(out var apiKey);
            return apiKey;
        }
    }
}