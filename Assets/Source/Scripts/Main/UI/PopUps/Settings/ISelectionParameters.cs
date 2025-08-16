using Source.Scripts.Core.Localization.Base;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal interface ISelectionParameters
    {
        string SettingName { get; }
        int SelectedIndex { get; }
        int[] SupportValues { get; }
        void SetValue(int enumIndex);
        string GetLocalization(ILocalizationKeysDatabase localizationKeysDatabase, int enumIndex);
    }
}