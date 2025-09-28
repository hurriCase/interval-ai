using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using Source.Scripts.Core.ApiHelper;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Translator
{
    [CreateAssetMenu(fileName = nameof(AzureTranslationConfig), menuName = nameof(AzureTranslationConfig))]
    internal sealed class AzureTranslationConfig : ApiConfigBase
    {
        [field: SerializeField] internal string Region { get; private set; }

        private string _languageCode;

        internal void SetLanguageCode(SystemLanguage targetLanguage)
        {
            _languageCode = targetLanguage.SystemLanguageToISO1();
        }

        internal override string GetApiUrl() => ZString.Format(endpointFormat, _languageCode);

        internal string ApiKey => GetApiKey();
    }
}