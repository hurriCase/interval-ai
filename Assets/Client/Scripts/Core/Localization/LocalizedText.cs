using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core.Localization
{
    /// <summary>
    ///     Localize text component.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        internal string LocalizationKey { get; private set; }

        private void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<Text>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}