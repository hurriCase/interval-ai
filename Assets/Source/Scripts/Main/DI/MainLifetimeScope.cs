using Source.Scripts.Core.AI;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Main.Data;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainWindowsController _windowsController;
        [SerializeField] private MenuBehaviour _menuBehaviour;

        [SerializeField] private ProgressDescriptionsDatabase _progressDescriptionsDatabase;

        [SerializeField] private ProgressGraphSettings _progressGraphSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterComponent(_menuBehaviour).AsImplementedInterfaces();

            builder.RegisterComponent(_progressDescriptionsDatabase).AsImplementedInterfaces();

            builder.RegisterComponent(_progressGraphSettings).AsImplementedInterfaces();

            builder.Register<GeminiAPI>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.Register<PracticeStateService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainCurrentWordsService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<CompleteStateService>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.RegisterEntryPoint<MainEntryPoint>();
        }
    }
}