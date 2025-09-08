using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3.Triggers;
using Source.Scripts.UI.Data;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours
{
    internal sealed class LanguageSettingBehaviour : MonoBehaviour
    {
        [field: SerializeField] internal LanguageToggle LeftThemeToggle { get; private set; }
        [field: SerializeField] internal LanguageToggle RightThemeToggle { get; private set; }

        [SerializeField] private Image _selectedImage;

        private IAnimationsConfig _animationsConfig;

        [Inject]
        public void Inject(IAnimationsConfig animationsConfig)
        {
            _animationsConfig = animationsConfig;
        }

        internal void Init()
        {
            LeftThemeToggle.OnPointerClickAsObservable()
                .SubscribeAndRegister(this, static self => self.MoveSelectedImage(0f));

            RightThemeToggle.OnPointerClickAsObservable()
                .SubscribeAndRegister(this, static self => self.MoveSelectedImage(1f));
        }

        private void MoveSelectedImage(float endValue)
        {
            var duration = _animationsConfig.SwitchComponentDuration;
            Tween.UIAnchorMin(
                _selectedImage.rectTransform,
                new Vector2(endValue, _selectedImage.rectTransform.anchorMin.y),
                duration);

            Tween.UIAnchorMax(
                _selectedImage.rectTransform,
                new Vector2(endValue, _selectedImage.rectTransform.anchorMax.y),
                duration);

            Tween.UIPivotX(_selectedImage.rectTransform, endValue, duration);
        }
    }
}