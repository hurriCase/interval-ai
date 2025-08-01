using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Audio;
using Source.Scripts.Core.Loader;
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

        [Inject] private IAddressablesLoader _addressablesLoader;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var audioHandler = await _addressablesLoader.LoadComponentAsync<AudioHandler>(_audioHandler, token);
            var audioHandlerInstance = Instantiate(audioHandler);

            DontDestroyOnLoad(audioHandlerInstance);
        }
    }
}