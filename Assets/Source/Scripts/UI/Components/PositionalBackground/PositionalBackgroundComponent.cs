using CustomUtils.Runtime.Attributes;
using CustomUtils.Runtime.CustomTypes.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components.PositionalBackground
{
    [RequireComponent(typeof(Image))]
    internal sealed class PositionalBackgroundComponent : MonoBehaviour
    {
        [SerializeField] private PositionType _position;
        [SerializeField] private EnumArray<PositionType, Sprite> _positionSprites = new(EnumMode.SkipFirst);

        [SerializeField, Self] private Image _image;

        private void OnValidate()
        {
            if (_position == PositionType.None)
                return;

            _image.sprite = _positionSprites[_position];
        }
    }
}