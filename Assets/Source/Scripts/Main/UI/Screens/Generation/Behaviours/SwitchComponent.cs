using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using PrimeTween;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.Screens.Generation.Behaviours
{
    internal sealed class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private ThemeToggle _leftThemeToggle;
        [SerializeField] private ThemeToggle _rightThemeToggle;

        [SerializeField] private Image _selectedImage;
        [SerializeField] private float _animationDuration;

        internal void Init()
        {
            _leftThemeToggle.OnPointerClickAsObservable()
                .Subscribe(this, static (_, component) => component.MoveSelectedImage(0f))
                .RegisterTo(destroyCancellationToken);

            _rightThemeToggle.OnPointerClickAsObservable()
                .Subscribe(this, static (_, component) => component.MoveSelectedImage(1f))
                .RegisterTo(destroyCancellationToken);
        }

        private void MoveSelectedImage(float endValue)
        {
            Tween.UIAnchorMin(
                _selectedImage.rectTransform,
                new Vector2(endValue, _selectedImage.rectTransform.anchorMin.y),
                _animationDuration);

            Tween.UIAnchorMax(
                _selectedImage.rectTransform,
                new Vector2(endValue, _selectedImage.rectTransform.anchorMax.y),
                _animationDuration);

            Tween.UIPivotX(
                _selectedImage.rectTransform,
                endValue,
                _animationDuration);
        }
    }
}