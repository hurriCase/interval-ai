using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal static class LocalizationKeyExtensions
    {
        internal static void SubscribeToText(this LocalizationKey key, TextMeshProUGUI text)
        {
            LocalizationController.Language
                .Subscribe((key, text), static (_, tuple) => tuple.text.text = tuple.key.GetLocalization())
                .RegisterTo(text.destroyCancellationToken);
        }

        internal static void SubscribeToText<T0>(
            this LocalizationKey key,
            TextMeshProUGUI text,
            T0 arg0)
        {
            LocalizationController.Language
                .Subscribe((key, text, arg0),
                    static (_, tuple) => tuple.text.SetTextFormat(tuple.key.GetLocalization(), tuple.arg0))
                .RegisterTo(text.destroyCancellationToken);
        }

        internal static void SubscribeToText<T0>(
            this Observable<T0> observable,
            LocalizationKey key,
            TextMeshProUGUI text)
        {
            LocalizationController.Language
                .CombineLatest(observable, static (_, arg0) => arg0)
                .Subscribe((key, text), static (arg0, tuple)
                    => tuple.text.SetTextFormat(tuple.key.GetLocalization(), arg0))
                .RegisterTo(text.destroyCancellationToken);
        }

        internal static void SubscribePluralToText(
            this PluralLocalization localization,
            int amount,
            TextMeshProUGUI text)
        {
            LocalizationController.Language
                .Subscribe((localization, amount, text),
                    static (language, tuple) => SetPluralText(tuple.localization, tuple.amount, language, tuple.text))
                .RegisterTo(text.destroyCancellationToken);
        }

        internal static void SubscribePluralToText(
            this Observable<int> observable,
            PluralLocalization localization,
            TextMeshProUGUI text)
        {
            LocalizationController.Language
                .CombineLatest(observable, static (language, amount) => (language, amount))
                .Subscribe((localization, text), static (tuple, observables)
                    => SetPluralText(observables.localization, tuple.amount, tuple.language, observables.text))
                .RegisterTo(text.destroyCancellationToken);
        }

        private static void SetPluralText(
            PluralLocalization localization,
            int amount,
            SystemLanguage language,
            TMP_Text text)
        {
            var localizationKey = PluralizationHelper.GetPluralForm(
                localization,
                amount,
                language);

            text.SetTextFormat(localizationKey.GetLocalization(), amount);
        }
    }
}
