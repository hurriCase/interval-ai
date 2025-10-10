using Source.Scripts.Core.Configs;
using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.ModuleState;
using Source.Scripts.Main.Data;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete;
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

            builder.Register<PracticeStateService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterStateFactory<PracticeState, IModuleStateService, ModuleStateService>(static (resolver,
                practiceState) => new ModuleStateService(
                resolver.Resolve<ICurrentWordsService>(),
                resolver.Resolve<IAppConfig>(),
                practiceState));

            builder.RegisterStateFactory<PracticeState, ICompleteStateService, CompleteStateService>(static (resolver,
                practiceState) => new CompleteStateService(
                resolver.Resolve<ICurrentWordsService>(),
                resolver.Resolve<IProgressRepository>(),
                resolver.Resolve<IWordsTimerService>(),
                practiceState));

            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainCurrentWordsService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<CompleteStateService>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.Register<CategorySelectionService>(Lifetime.Scoped).AsSelf();
            builder.Register<WordCategorySelectionService>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<MainEntryPoint>();
        }
    }
}