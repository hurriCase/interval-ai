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
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();

        public PersistentReactiveProperty<ThemeType> ThemeType { get; } = new();

        public PersistentReactiveProperty<bool> IsSendNotifications { get; } = new();
        public PersistentReactiveProperty<bool> IsShowTranscription { get; } = new();
        public PersistentReactiveProperty<bool> IsSwipeEnabled { get; } = new();

        private DisposableBag _disposableBag;

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                ThemeType.InitAsync(
                    PersistentKeys.CurrentThemeKey,
                    cancellationToken,
                    AndroidThemeDetector.GetAndroidSystemTheme()),

                IsSendNotifications.InitAsync(PersistentKeys.IsSendNotificationsKey, cancellationToken),
                IsShowTranscription.InitAsync(PersistentKeys.IsShowTranscriptionKey, cancellationToken),
                IsSwipeEnabled.InitAsync(PersistentKeys.IsSwipeEnabledKey, cancellationToken)
            };

            await UniTask.WhenAll(initTasks);

            ThemeType
                .Subscribe(newTheme => ThemeHandler.Instance.CurrentThemeType.Value = newTheme)
                .AddTo(ref _disposableBag);
        }

        public void Dispose()
        {
            _disposableBag.Dispose();
            CurrentCulture.Dispose();
            ThemeType.Dispose();
            IsSendNotifications.Dispose();
            IsShowTranscription.Dispose();
            IsSwipeEnabled.Dispose();
        }
    }
}