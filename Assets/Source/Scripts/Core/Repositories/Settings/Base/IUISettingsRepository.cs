using System.Globalization;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.Theme;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface IUISettingsRepository
    {
        PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        PersistentReactiveProperty<ThemeType> ThemeType { get; }
        PersistentReactiveProperty<bool> IsSendNotifications { get; }
        PersistentReactiveProperty<bool> IsShowTranscription { get; }
        PersistentReactiveProperty<bool> IsSwipeEnabled { get; }
    }
}