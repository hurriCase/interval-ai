using Cysharp.Text;
using Source.Scripts.Core.ApiHelper;
using UnityEngine;

namespace Source.Scripts.Core.GenerativeLanguage
{
    [CreateAssetMenu(
        fileName = nameof(GeminiGenerativeLanguageConfig),
        menuName = nameof(GeminiGenerativeLanguageConfig)
    )]
    internal sealed class GeminiGenerativeLanguageConfig : ApiConfigBase
    {
        [SerializeField] private string _endpointFormat;
        [SerializeField] private string _modelName;

        internal string GetApiUrl() => ZString.Format(_endpointFormat, _modelName, ApiKey);
    }
}