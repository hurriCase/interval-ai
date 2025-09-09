using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    [CreateAssetMenu(fileName = nameof(GoogleTTSConfig), menuName = nameof(GoogleTTSConfig))]
    internal sealed class GoogleTTSConfig : ScriptableObject
    {
        [field: SerializeField] internal string ApiKey { get; private set; }
    }
}