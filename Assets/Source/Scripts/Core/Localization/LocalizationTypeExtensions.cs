using CustomUtils.Runtime.Localization;

namespace Source.Scripts.Core.Localization
{
    internal static class LocalizationTypeExtensions
    {
        internal static string GetLocalization(this LocalizationType type)
            => LocalizationController.Localize(LocalizationKeysDatabase.Instance.GetLocalization(type));
    }
}