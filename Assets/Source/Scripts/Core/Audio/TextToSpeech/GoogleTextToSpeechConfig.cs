using Cysharp.Text;
using Source.Scripts.Core.ApiHelper;
using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    [CreateAssetMenu(fileName = nameof(GoogleTextToSpeechConfig), menuName = nameof(GoogleTextToSpeechConfig))]
    internal sealed class GoogleTextToSpeechConfig : ApiConfigBase
    {
        internal override string GetApiUrl() => ZString.Format(endpointFormat, GetApiKey());
    }
}