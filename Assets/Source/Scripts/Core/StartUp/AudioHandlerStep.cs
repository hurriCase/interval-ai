using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.StartUp
{
    [CreateAssetMenu(fileName = nameof(AudioHandlerStep), menuName = nameof(AudioHandlerStep))]
    internal sealed class AudioHandlerStep : StepBase
    {
        [SerializeField] private AssetReferenceT<AudioHandler> _audioHandler;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var audioHandler = await PrefabLoader.LoadComponentAsync<AudioHandler>(_audioHandler, token);
            var audioHandlerInstance = Instantiate(audioHandler);

            DontDestroyOnLoad(audioHandlerInstance);
        }
    }
}