using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.ImagePixelsPerUnit
{
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    internal sealed class ImagePixelsPerUnitAdjuster : MonoBehaviour
    {
        [SerializeField] private BackgroundType _backgroundType;

        private Image _image;
        private RectTransform _rectTransform;
        private AspectRatioFitter _parentAspectRatioFitter;

        private void OnEnable()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();

            if (transform.parent)
                _parentAspectRatioFitter = transform.parent.GetComponent<AspectRatioFitter>();

            UpdateImagePixelPerUnit();
        }

        private void Update()
        {
            if (!_parentAspectRatioFitter && transform.parent)
                _parentAspectRatioFitter = transform.parent.GetComponent<AspectRatioFitter>();

            UpdateImagePixelPerUnit();
        }

        private void UpdateImagePixelPerUnit()
        {
            if (!_image || !_image.sprite || !_parentAspectRatioFitter)
                return;

            var imageWidth = _image.sprite.rect.width;
            var elementImageRatio = _rectTransform.rect.width / imageWidth;
            var aspectRatio = _parentAspectRatioFitter.aspectRatio;

            var additionalPixelPerUnitMultiplier = _backgroundType switch
            {
                BackgroundType.None => 1f,
                BackgroundType.Button => 1f,
                BackgroundType.Block => 2f,
                _ => 1f
            };

            _image.pixelsPerUnitMultiplier = aspectRatio / elementImageRatio * additionalPixelPerUnitMultiplier;
        }
    }
}