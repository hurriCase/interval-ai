using Cysharp.Text;
using Source.Scripts.Core.ApiHelper;
using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    [CreateAssetMenu(fileName = nameof(GoogleTextToSpeechConfig), menuName = nameof(GoogleTextToSpeechConfig))]
    internal sealed class GoogleTextToSpeechConfig : ApiConfigBase
    {
        [SerializeField] private string _endpointFormat;

        internal string GetApiUrl() => ZString.Format(_endpointFormat, ApiKey);
    }
}