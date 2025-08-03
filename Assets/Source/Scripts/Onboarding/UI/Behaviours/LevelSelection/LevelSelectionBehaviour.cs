using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.Behaviours.LevelSelection
{
    internal sealed class LevelSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private ToggleGroup _selectionToggleGroup;
        [SerializeField] private SelectionToggleItem _selectionToggleItem;
        [SerializeField] private RectTransform _levelButtonsContainer;
        [SerializeField] private AspectRatioFitter _aspectRatioFitter;
        [SerializeField] private float _spacingRatio;

        [SerializeField] private EnumArray<LanguageLevel, AssetReferenceT<Sprite>> _levelLanguageIcons =
            new(EnumMode.SkipFirst);

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationDatabase _localizationDatabase;
        [Inject] private IAddressablesLoader _addressablesLoader;
        [Inject] private IObjectResolver _objectResolver;

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in _localizationDatabase.LanguageLevelKeys.AsTuples())
            {
                var createdButton = _objectResolver.Instantiate(_selectionToggleItem, _levelButtonsContainer);
                createdButton.LevelText.text = levelLocalizationKey.GetLocalization();
                createdButton.CheckboxComponent.OnPointerClickAsObservable()
                    .Subscribe((levelType, _settingsRepository),
                        static (_, tuple) => tuple._settingsRepository.LanguageLevel.Value = tuple.levelType)
                    .RegisterTo(destroyCancellationToken);
                createdButton.CheckboxComponent.group = _selectionToggleGroup;

                SetLevelIcon(createdButton.Icon, _levelLanguageIcons[levelType].AssetGUID).Forget();

                _aspectRatioFitter.CreateSpacing(_spacingRatio, _levelButtonsContainer,
                    AspectRatioFitter.AspectMode.WidthControlsHeight);
            }
        }

        private async UniTask SetLevelIcon(Image image, string assetGUID)
        {
            image.sprite = await _addressablesLoader.LoadAsync<Sprite>(assetGUID, destroyCancellationToken);
        }
    }
}