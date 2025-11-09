using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization.LocalizationTypes;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    [CreateAssetMenu(fileName = nameof(CompleteLocalizationDataTemp), menuName = nameof(CompleteLocalizationDataTemp))]
    internal sealed class CompleteLocalizationDataTemp : ScriptableObject
    {
        [field: SerializeField] internal LocalizationKey ButtonPositive { get; private set; }
        [field: SerializeField] internal LocalizationKey ButtonNegative { get; private set; }

        [field: SerializeField]
        internal EnumArray<CompleteType, LocalizationKey> DescriptionLocalizations { get; private set; }

        internal string GetDescription(CompleteType completeType)
            => DescriptionLocalizations[completeType].GetLocalization();
    }
}