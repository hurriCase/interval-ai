using CustomUtils.Runtime.AddressableSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Shared
{
    internal sealed class DescriptiveImageBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _descriptiveImage;

        private IAddressablesLoader _addressablesLoader;

        [Inject]
        internal void Inject(IAddressablesLoader addressablesLoader)
        {
            _addressablesLoader = addressablesLoader;
        }

        internal void UpdateView(CachedSprite cachedSprite)
        {
            gameObject.SetActive(cachedSprite.IsValid);
            _addressablesLoader.AssignImageAsync(_descriptiveImage, cachedSprite, destroyCancellationToken);
        }
    }
}