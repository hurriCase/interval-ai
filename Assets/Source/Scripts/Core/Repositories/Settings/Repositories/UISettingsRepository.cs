using System;
using System.Globalization;
using System.Threading;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.Theme;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.Core.Repositories.Settings.Repositories
{
    internal sealed class UISettingsRepository : IUISettingsRepository, IRepository, IDisposable
    {
        public PersistentReactiveProperty<ThemeType> ThemeType { get; } = new();
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();
        public PersistentReactiveProperty<int> MaxShownExamples { get; } = new();
        public PersistentReactiveProperty<bool> IsSendNotifications { get; } = new();
        public PersistentReactiveProperty<bool> IsShowTranscription { get; } = new();
        public PersistentReactiveProperty<bool> IsSwipeEnabled { get; } = new();

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;
        private IDisposable _disposable;

        internal UISettingsRepository(IDefaultSettingsConfig defaultSettingsConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                ThemeType.InitAsync(
                    PersistentKeys.CurrentThemeKey,
                    token,
                    AndroidThemeDetector.GetAndroidSystemTheme()),

                MaxShownExamples.InitAsync(
                    PersistentKeys.MaxShownExamplesKey,
                    token,
                    _defaultSettingsConfig.MaxShownExamples),

                CurrentCulture.InitAsync(PersistentKeys.CurrentCultureKey, token, CultureInfo.CurrentCulture),
                IsSendNotifications.InitAsync(PersistentKeys.IsSendNotificationsKey, token),
                IsShowTranscription.InitAsync(PersistentKeys.IsShowTranscriptionKey, token),
                IsSwipeEnabled.InitAsync(PersistentKeys.IsSwipeEnabledKey, token, _defaultSettingsConfig.IsSwipeEnabled)
            };

            await UniTask.WhenAll(initTasks);

            _disposable = ThemeType.Subscribe(newTheme => ThemeHandler.CurrentThemeType.Value = newTheme);
        }

        public void Dispose()
        {
            _disposable.Dispose();
            CurrentCulture.Dispose();
            ThemeType.Dispose();
            IsSendNotifications.Dispose();
            IsShowTranscription.Dispose();
            IsSwipeEnabled.Dispose();
        }
    }
}