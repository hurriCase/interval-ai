using System;
using Client.Scripts.UI.Theme.Base;
using TMPro;
using UnityEngine;

namespace Client.Scripts.UI.Theme.Components
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TMP_Text))]
    internal sealed class TextThemeComponent : BaseThemeComponent<TMP_Text>
    {
        public override void ApplyColor()
        {
            switch (ColorType)
            {
                case ColorType.Shared:
                    _targetComponent.color = ThemeSharedColor.Color;
                    break;

                case ColorType.SolidColor:
                    _targetComponent.color = GetCurrentSolidColor();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}