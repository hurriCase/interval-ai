using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.CustomTypes.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components.PositionalBackground
{
    [RequireComponent(typeof(Image))]
    internal sealed class PositionalBackgroundComponent : ImageBehaviour
    {
        [SerializeField] private PositionType _position;
        [SerializeField] private EnumArray<PositionType, Sprite> _positionSprites = new(EnumMode.SkipFirst);

        private void OnValidate()
        {
            if (_position == PositionType.None)
                return;

            Image.sprite = _positionSprites[_position];
        }
    }
}