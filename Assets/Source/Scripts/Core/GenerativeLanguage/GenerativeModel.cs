using Cysharp.Text;
using UnityEngine;

namespace Source.Scripts.Core.GenerativeLanguage
{
    [CreateAssetMenu(
        fileName = nameof(GeminiGenerativeLanguageConfig),
        menuName = nameof(GeminiGenerativeLanguageConfig)
    )]
    internal sealed class GeminiGenerativeLanguageConfig : ScriptableObject
    {
        [SerializeField] private string _endpointFormat;
        [SerializeField] private string _modelName;
        [SerializeField] private string _apiKey;

        internal string GetApiUrl() => ZString.Format(_endpointFormat, _modelName, _apiKey);
    }
}