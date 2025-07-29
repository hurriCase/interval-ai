using Source.Scripts.Core.AI;
using Source.Scripts.Main.Source.Scripts.Main.Data;
using Source.Scripts.Main.Source.Scripts.Main.Data.Base;
using Source.Scripts.UI.Windows;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.Source.Scripts.Main.DI
{
    internal sealed class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private MenuBehaviour _menuBehaviour;

        [SerializeField] private LocalizationKeysDatabase _localizationKeysDatabase;
        [SerializeField] private ProgressDescriptionsDatabase _progressDescriptionsDatabase;

        [SerializeField] private ProgressGraphSettings _progressGraphSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).As<IWindowsController>();
            builder.RegisterComponent(_menuBehaviour).As<IMenuBehaviour>();

            builder.RegisterComponent(_localizationKeysDatabase).As<ILocalizationKeysDatabase>();
            builder.RegisterComponent(_progressDescriptionsDatabase).As<IProgressDescriptionsDatabase>();

            builder.RegisterComponent(_progressGraphSettings).As<IProgressGraphSettings>();

            builder.Register<GeminiAPI>(Lifetime.Singleton).As<IAIController>();

            builder.RegisterEntryPoint<MainEntryPoint>();
        }
    }
}