using Cysharp.Threading.Tasks;
using PrimeTween;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.PopUps
{
    internal sealed class SelectionVisibilityHandler : PopUpVisibilityHandler
    {
        [SerializeField] private RectTransform _selectionsContainer;
        [SerializeField] private RectTransform _targetHeightRect;
        [SerializeField] private RectTransform _scrollView;
        [SerializeField] private RectTransform _viewPort;
        [SerializeField] private TweenSettings _showSettings;
        [SerializeField] private float _maxHeightPercent;

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            var maxHeight = _targetHeightRect.rect.height * _maxHeightPercent;
            var preferredHeight = _selectionsContainer.rect.height - _viewPort.rect.size.y;
            var targetHeight = Mathf.Min(maxHeight, preferredHeight);
            await Tween.UISizeDelta(_scrollView, new Vector2(_scrollView.sizeDelta.x, targetHeight), _showSettings);
        }

        internal override async UniTask HideAsync()
        {
            await Tween.UISizeDelta(_scrollView, new Vector2(_scrollView.sizeDelta.x, 0), _showSettings);

            base.HideAsync().Forget();
        }
    }
}