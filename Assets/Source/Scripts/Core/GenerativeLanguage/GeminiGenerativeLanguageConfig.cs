using Cysharp.Text;
using Source.Scripts.Core.Api.Base;
using UnityEngine;

namespace Source.Scripts.Core.GenerativeLanguage
{
    [CreateAssetMenu(
        fileName = nameof(GeminiGenerativeLanguageConfig),
        menuName = nameof(GeminiGenerativeLanguageConfig)
    )]
    internal sealed class GeminiGenerativeLanguageConfig : ApiConfigBase
    {
        [SerializeField] private string _modelName;

        internal override bool IsValidUrl() => base.IsValidUrl() && string.IsNullOrEmpty(_modelName) is false;
        internal override string GetApiUrl() => ZString.Format(endpointFormat, _modelName, GetApiKey());
    }
}