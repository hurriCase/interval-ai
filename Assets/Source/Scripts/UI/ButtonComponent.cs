using CustomUtils.Runtime.CustomBehaviours;
using R3;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    internal sealed class ButtonComponent : ButtonBehaviour
    {
        private void Awake()
        {
            Button.OnClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .AddTo(this);
        }
    }
}