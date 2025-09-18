using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Components.Animation;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours
{
    internal sealed class LanguageSettingBehaviour : MonoBehaviour
    {
        [field: SerializeField] internal EnumArray<LanguageType, LanguageToggle> Toggles { get; private set; } =
            new(EnumMode.SkipFirst);
        [field: SerializeField] internal AnchoredPositionAnimation<LanguageType> PositionAnimation { get; private set; }

        internal void Init()
        {
            foreach (var (languageType, languageToggle) in Toggles.AsTuples())
            {
                languageToggle.Init(languageType);
                languageToggle.OnPointerClickAsObservable()
                    .SubscribeAndRegister(this, languageType, static (languageType, self)
                        => self.PositionAnimation.PlayAnimation(languageType));
            }
        }
    }
}