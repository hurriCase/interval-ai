using CustomUtils.Runtime.UI.RatioLayout;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.ImagePixelsPerUnit
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    internal sealed class ImagePixelsPerUnitAdjuster : MonoBehaviour
    {
        [SerializeField] private BackgroundType _backgroundType;

        private Image _image;
        private RectTransform _rectTransform;
        private AspectRatioFitter _parentAspectRatioFitter;
        private RatioLayoutElement _ratioLayoutElement;

        private void OnEnable()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();

            AssignRatioComponent();

            UpdateImagePixelPerUnit();
        }

        private void Update()
        {
            UpdateImagePixelPerUnit();
        }

        private void AssignRatioComponent()
        {
            if (!transform.parent)
                return;

            if (_parentAspectRatioFitter || _ratioLayoutElement)
                return;

            if (transform.parent.TryGetComponent(out _parentAspectRatioFitter) is false)
                transform.parent.TryGetComponent(out _ratioLayoutElement);
        }

        private void UpdateImagePixelPerUnit()
        {
            if (!_image || !_image.sprite || (!_parentAspectRatioFitter && !_ratioLayoutElement))
                return;

            var imageWidth = _image.sprite.rect.width;
            var elementImageRatio = _rectTransform.rect.width / imageWidth;
            if (TryGetAspectRatio(out var aspectRatio) is false)
                return;

            var additionalPixelPerUnitMultiplier = _backgroundType switch
            {
                BackgroundType.None => 1f,
                BackgroundType.Button => 1f,
                BackgroundType.Block => 2f,
                _ => 1f
            };

            _image.pixelsPerUnitMultiplier = aspectRatio / elementImageRatio * additionalPixelPerUnitMultiplier;
        }

        private bool TryGetAspectRatio(out float ratio)
        {
            ratio = 0;
            if (_parentAspectRatioFitter)
            {
                ratio = _parentAspectRatioFitter.aspectRatio;
                return true;
            }

            if (!_ratioLayoutElement)
                return false;

            ratio = _ratioLayoutElement.AspectRatio;
            return true;
        }
    }
}