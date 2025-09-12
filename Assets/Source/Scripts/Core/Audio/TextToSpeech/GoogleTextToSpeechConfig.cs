using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    [CreateAssetMenu(fileName = nameof(GoogleTextToSpeechConfig), menuName = nameof(GoogleTextToSpeechConfig))]
    internal sealed class GoogleTextToSpeechConfig : ScriptableObject
    {
        [field: SerializeField] internal string ApiKey { get; private set; }
    }
}