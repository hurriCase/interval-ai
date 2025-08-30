using UnityEngine;

namespace Source.Scripts.UI.Data
{
    [CreateAssetMenu(fileName = nameof(AnimationsConfig), menuName = nameof(AnimationsConfig))]
    internal sealed class AnimationsConfig : ScriptableObject, IAnimationsConfig
    {
        [field: SerializeField] public float PopUpShowDuration { get; private set; }
        [field: SerializeField] public float PopUpHideDuration { get; private set; }
        [field: SerializeField] public float SelectionSwitchDuration { get; private set; }
    }
}