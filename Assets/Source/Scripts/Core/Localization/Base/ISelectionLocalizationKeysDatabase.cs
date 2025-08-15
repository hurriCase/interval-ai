namespace Source.Scripts.Core.Localization.Base
{
    internal interface ISelectionLocalizationKeysDatabase
    {
        string GetLocalization<TEnum>(TEnum enumValue);
    }
}