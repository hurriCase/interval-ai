using System.Threading;
using CustomUtils.Runtime.AddressableSystem;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Audio.Sounds;
using Source.Scripts.Core.Audio.Sounds.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(AudioHandlerStep),
        menuName = InitializationStepsPath + nameof(AudioHandlerStep)
    )]
    internal sealed class AudioHandlerStep : StepBase
    {
        [SerializeField] private AssetReferenceT<AudioHandler> _audioHandler;

        private IAddressablesLoader _addressablesLoader;
        private IAudioHandlerProvider _audioHandlerProvider;

        [Inject]
        internal void Inject(IAddressablesLoader addressablesLoader, IAudioHandlerProvider audioHandlerProvider)
        {
            _addressablesLoader = addressablesLoader;
            _audioHandlerProvider = audioHandlerProvider;
        }

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var audioHandler = await _addressablesLoader.LoadComponentAsync<AudioHandler>(_audioHandler, token);
            var audioHandlerInstance = Instantiate(audioHandler);

            DontDestroyOnLoad(audioHandlerInstance);
            await audioHandlerInstance.InitAsync(cancellationToken: token);

            _audioHandlerProvider.SetAudioHandler(audioHandlerInstance);
        }
    }
}