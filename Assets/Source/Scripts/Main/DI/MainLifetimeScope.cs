using Source.Scripts.Core.AI;
using Source.Scripts.Main.Data;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Menu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private MenuBehaviour _menuBehaviour;

        [SerializeField] private ProgressDescriptionsDatabase _progressDescriptionsDatabase;

        [SerializeField] private ProgressGraphSettings _progressGraphSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterComponent(_menuBehaviour).AsImplementedInterfaces();

            builder.RegisterComponent(_progressDescriptionsDatabase).AsImplementedInterfaces();

            builder.RegisterComponent(_progressGraphSettings).AsImplementedInterfaces();

            builder.Register<GeminiAPI>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PracticeStateService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterEntryPoint<MainEntryPoint>();
        }
    }
}