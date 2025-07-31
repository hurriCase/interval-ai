using CustomUtils.Runtime.Audio;
using UnityEngine;

namespace Source.Scripts.Core.Audio
{
    [CreateAssetMenu(
        fileName = AudioDatabaseAssetName,
        menuName = AudioDatabaseAssetMenuPath
    )]
    internal sealed class AudioDatabase : AudioDatabaseGeneric<MusicType, SoundType>
    {
        private const string AudioDatabaseAssetMenuPath = SoundResourcesPathPrefix + "/" + AudioDatabaseAssetName;
        private const string AudioDatabaseAssetName = "AudioDatabase";

        private const string SoundResourcesPathPrefix = "Sounds";
    }
}