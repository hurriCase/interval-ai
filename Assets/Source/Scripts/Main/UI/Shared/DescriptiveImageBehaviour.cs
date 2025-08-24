using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Sprites;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Shared
{
    internal sealed class DescriptiveImageBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _descriptiveImage;

        [Inject] private IAddressablesLoader _addressablesLoader;

        internal void UpdateView(CachedSprite cachedSprite)
        {
            gameObject.SetActive(cachedSprite.IsValid);
            _addressablesLoader.AssignImageAsync(_descriptiveImage, cachedSprite, destroyCancellationToken);
        }
    }
}