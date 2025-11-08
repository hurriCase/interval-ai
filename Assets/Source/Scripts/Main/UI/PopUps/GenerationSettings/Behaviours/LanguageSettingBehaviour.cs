using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.PopUps.Selection.LocalizationData;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours
{
    internal sealed class LanguageSettingBehaviour : MonoBehaviour
    {
        [field: SerializeField] internal EnumArray<LanguageType, LanguageToggle> Toggles { get; private set; } =
            new(EnumMode.SkipFirst);
        [field: SerializeField] internal AnchoredPositionAnimation<LanguageType> PositionAnimation { get; private set; }

        [SerializeField] private LanguageLocalizationConfig _languageLocalizationConfig;

        internal void Init()
        {
            foreach (var (languageType, languageToggle) in Toggles.AsTuples())
            {
                languageToggle.Init(_languageLocalizationConfig, languageType);
                languageToggle.OnPointerClickAsObservable()
                    .SubscribeUntilDestroy(this, languageType, static (languageType, self)
                        => self.PositionAnimation.PlayAnimation(languageType));
            }
        }
    }
}