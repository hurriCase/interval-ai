using Cysharp.Text;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Translator
{
    [CreateAssetMenu(fileName = nameof(AzureTranslationConfig), menuName = nameof(AzureTranslationConfig))]
    internal sealed class AzureTranslationConfig : ScriptableObject
    {
        [field: SerializeField] internal string SubscriptionKey { get; private set; }
        [field: SerializeField] internal string Region { get; private set; }

        [SerializeField] private string _endpointFormat;

        internal string GetApiUrl(string languageCode) => ZString.Format(_endpointFormat, languageCode);
    }
}