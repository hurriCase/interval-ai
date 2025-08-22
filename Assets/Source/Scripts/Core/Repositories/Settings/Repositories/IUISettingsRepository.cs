using System;
using System.Globalization;
using System.Threading;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.Theme.Base;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.Core.Repositories.Settings.Repositories
{
    internal sealed class UISettingsRepository : IUISettingsRepository, IRepository, IDisposable
    {
        public PersistentReactiveProperty<ThemeType> ThemeType { get; } = new();
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();
        public PersistentReactiveProperty<bool> IsSendNotifications { get; } = new();
        public PersistentReactiveProperty<bool> IsShowTranscription { get; } = new();
        public PersistentReactiveProperty<bool> IsSwipeEnabled { get; } = new();

        private IDisposable _disposable;

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                ThemeType.InitAsync(
                    PersistentKeys.CurrentThemeKey,
                    token,
                    AndroidThemeDetector.GetAndroidSystemTheme()),

                CurrentCulture.InitAsync(PersistentKeys.CurrentCultureKey, token, CultureInfo.CurrentCulture),
                IsSendNotifications.InitAsync(PersistentKeys.IsSendNotificationsKey, token),
                IsShowTranscription.InitAsync(PersistentKeys.IsShowTranscriptionKey, token),
                IsSwipeEnabled.InitAsync(PersistentKeys.IsSwipeEnabledKey, token)
            };

            await UniTask.WhenAll(initTasks);

            _disposable = ThemeType
                .Subscribe(newTheme => ThemeHandler.Instance.CurrentThemeType.Value = newTheme);
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