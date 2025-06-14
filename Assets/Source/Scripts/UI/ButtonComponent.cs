using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    internal sealed class ButtonComponent : RequiredBehaviour<Button>
    {
        private void Awake()
        {
            Button.onClick.AddListener(() => AudioHandler.Instance.PlayOneShotSound(SoundType.Button));
        }
    }
}