using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Categories.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryContainerItem : MonoBehaviour
    {
        [field: SerializeField] internal RectTransform CategoryContainer { get; private set; }

        [SerializeField] private TextMeshProUGUI _titleText;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        private CategoryType _currentCategoryType;

        internal void Init(CategoryType categoryType)
        {
            _currentCategoryType = categoryType;

            LocalizationController.Language.SubscribeAndRegister(this, static self => self.UpdateTitleText());
        }

        private void UpdateTitleText()
        {
            _titleText.text = _localizationKeysDatabase.GetLearningStateLocalization(_currentCategoryType);
        }
    }
}