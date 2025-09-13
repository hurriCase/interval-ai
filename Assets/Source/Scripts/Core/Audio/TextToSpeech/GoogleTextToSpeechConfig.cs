using Cysharp.Text;
using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    [CreateAssetMenu(fileName = nameof(GoogleTextToSpeechConfig), menuName = nameof(GoogleTextToSpeechConfig))]
    internal sealed class GoogleTextToSpeechConfig : ScriptableObject
    {
        [SerializeField] private string _endpointFormat;
        [SerializeField] private string _apiKey;

        internal string GetApiUrl() => ZString.Format(_endpointFormat, _apiKey);
    }
}