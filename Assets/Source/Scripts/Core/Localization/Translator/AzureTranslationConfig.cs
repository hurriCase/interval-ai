using Cysharp.Text;
using Source.Scripts.Core.ApiHelper;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Translator
{
    [CreateAssetMenu(fileName = nameof(AzureTranslationConfig), menuName = nameof(AzureTranslationConfig))]
    internal sealed class AzureTranslationConfig : ApiConfigBase
    {
        [field: SerializeField] internal string Region { get; private set; }

        [SerializeField] private string _endpointFormat;

        internal string GetApiUrl(string languageCode) => ZString.Format(_endpointFormat, languageCode);
    }
}