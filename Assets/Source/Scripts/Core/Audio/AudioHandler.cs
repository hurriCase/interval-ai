using CustomUtils.Runtime.Audio;
using CustomUtils.Runtime.CustomTypes.Singletons;

namespace Source.Scripts.Core.Audio
{
    internal sealed class AudioHandler : PersistentSingletonBehavior<AudioHandlerBase<MusicType, SoundType>> { }
}