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

        internal override bool IsValid() => base.IsValid() && string.IsNullOrEmpty(_modelName) is false;
        internal override string GetApiUrl() => ZString.Format(endpointFormat, _modelName, GetApiKey());
    }
}