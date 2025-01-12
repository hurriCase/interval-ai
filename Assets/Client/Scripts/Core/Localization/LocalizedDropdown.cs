﻿using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core.Localization
{
    /// <summary>
    ///     Localize dropdown component.
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    internal class LocalizedDropdown : MonoBehaviour
    {
        internal string[] LocalizationKeys { get; private set; }

        internal void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        internal void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            var dropdown = GetComponent<Dropdown>();

            for (var i = 0; i < LocalizationKeys.Length; i++)
                dropdown.options[i].text = LocalizationManager.Localize(LocalizationKeys[i]);

            if (dropdown.value < LocalizationKeys.Length)
                dropdown.captionText.text = LocalizationManager.Localize(LocalizationKeys[dropdown.value]);
        }
    }
}