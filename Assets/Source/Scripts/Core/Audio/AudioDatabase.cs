using CustomUtils.Runtime.Audio;
using UnityEngine;

namespace Source.Scripts.Core.Audio
{
    [CreateAssetMenu(
        fileName = ResourcePaths.AudioDatabaseAssetName,
        menuName = ResourcePaths.AudioDatabaseAssetMenuPath
    )]
    internal sealed class AudioDatabase : AudioDatabaseGeneric<MusicType, SoundType> { }
}